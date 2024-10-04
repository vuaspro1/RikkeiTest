using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;
using RikkeiTest.Repository;

namespace RikkeiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;
        private readonly MyDbContext myDbContext;
        public RoleController(IRoleRepository roleRepository, IMapper mapper, MyDbContext myDbContext)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
            this.myDbContext = myDbContext;
        }
        [HttpPost]
        public IActionResult CreateRole([FromBody] RoleDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest();
            var role = myDbContext.Roles
                .Where(item => item.Name.Trim().ToUpper() == roleCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (role != null)
            {
                ModelState.AddModelError("", "This role already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleMap = mapper.Map<Role>(roleCreate);

            if (!roleRepository.CreateRole(roleMap))
            {
                return BadRequest("Error");
            }
            return Ok(roleMap);
        }
    }
}
