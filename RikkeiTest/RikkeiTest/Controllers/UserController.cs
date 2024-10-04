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
    public class UserController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserController(MyDbContext myDbContext, IMapper mapper,
            IUserRepository userRepository) 
        {
            this.myDbContext = myDbContext;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        //[HttpGet]
        //[Route("GetUser")]
        //public List<User> GetUsers()
        //{
        //    return myDbContext.Users.ToList();
        //}

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userCreate) 
        {
            if (userCreate == null)
                return BadRequest();
            var userByUserName = userRepository.GetUsersToCheck().FirstOrDefault(item => item.UserName == userCreate.UserName);

            if (userByUserName != null)
            {
                ModelState.AddModelError("", "UserName already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = mapper.Map<User>(userCreate);

            if (!userRepository.CreateUser(userMap))
            {
                return BadRequest("Error");
            }
            return Ok(userMap);
        }
    }
}
