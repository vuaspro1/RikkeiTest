using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RikkeiTest.Interface;
using RikkeiTest.Models;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RikkeiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        private readonly AppSettings appSettings;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        
        public LoginController(MyDbContext myDbContext, IOptionsMonitor<AppSettings> optionsMonitor,
            IUserRepository userRepository, IRoleRepository roleRepository) 
        {
            this.myDbContext = myDbContext;
            appSettings = optionsMonitor.CurrentValue;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        [HttpGet("GetMe")]
        [Authorize]
        public async Task<IActionResult> GetMe() 
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(idClaim))
            {
                return BadRequest("Unable to retrieve user ID.");
            }

            if (int.TryParse(idClaim, out var id))
            {
                var user = await userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(user);
            }

            return BadRequest("Invalid user ID format.");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Validate(LoginModel loginModel) 
        {
            var user = userRepository.Authenticate(loginModel.UserName, loginModel.Password);
            if (user == null) 
            {
                return BadRequest("Invalid username or password.");
            }
            var token = await GenerateToken(user);
            return Ok(token);
        }

        private async Task<TokenModel> GenerateToken(User user) 
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserName", user.UserName),
                new Claim("Id", user.Id.ToString()),
            }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                (secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            var userId = user.Id;
            var roleName = roleRepository.GetRolesByUser(userId);

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };
            await myDbContext.AddAsync(refreshTokenEntity);
            await myDbContext.SaveChangesAsync();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Roles = roleName,
                UserName = user.UserName,
                Name = user.Name,
                UserId = userId,
            };
        }
        private string GenerateRefreshToken()
        {
            var random = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken,
                    tokenValidateParam, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                        StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Ok("Invalid token");
                    }
                }

                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x =>
                x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConverUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok("Access token has not yet expired");
                }
                var storedToken = myDbContext.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return Ok("Refresh token does not exist");
                }

                if (storedToken.IsUsed)
                {
                    return Ok("Refresh token has been used");
                }

                if (storedToken.IsRevoked)
                {
                    return Ok("Refresh token has been revoked");
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type ==
                JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok("Token doesn't match");
                }

                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                myDbContext.Update(storedToken);
                await myDbContext.SaveChangesAsync();

                var user = await myDbContext.Users.SingleOrDefaultAsync(u => u.Id == storedToken.UserId);
                var token = await GenerateToken(user);

                return Ok(token);
            }
            catch
            {
                return BadRequest("Error");
            }
        }
        private DateTime ConverUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
