using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        public UserRoleController(IUserRoleRepository userRoleRepository, IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRoleRepository = userRoleRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        [HttpPost]
        public IActionResult CreateUserRole([FromBody] UserRoleDto userRoleCreate)
        {
            if (userRoleCreate == null)
                return BadRequest();

            if (userRoleRepository.UserRoleExists(userRoleCreate.UserId, userRoleCreate.RoleId))
                return BadRequest("UserRole already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRoleMap = mapper.Map<UserRole>(userRoleCreate);
            userRoleMap.User = userRepository.GetUserToCheck(userRoleCreate.UserId);
            userRoleMap.Role = roleRepository.GetRoleToCheck(userRoleCreate.RoleId);

            if (!userRoleRepository.CreateUserRole(userRoleMap))
            {
                return BadRequest("Error");
            }
            var createdUserRole = mapper.Map<UserRoleDto>(userRoleMap);
            return Ok(createdUserRole);
        }
    }
}
