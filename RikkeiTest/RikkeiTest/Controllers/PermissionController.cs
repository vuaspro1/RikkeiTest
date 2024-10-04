using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;
using RikkeiTest.Repository;

namespace RikkeiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IMapper mapper;
        private readonly MyDbContext myDbContext;
        public PermissionController(IPermissionRepository permissionRepository, IMapper mapper, MyDbContext myDbContext)
        {
            this.permissionRepository = permissionRepository;
            this.mapper = mapper;
            this.myDbContext = myDbContext;
        }
        [HttpPost]
        public IActionResult CreateRole([FromBody] PermissionDto permissionCreate)
        {
            if (permissionCreate == null)
                return BadRequest();
            var role = myDbContext.Roles
                .Where(item => item.Name.Trim().ToUpper() == permissionCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (role != null)
            {
                ModelState.AddModelError("", "This role already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var permissioMap = mapper.Map<Permission>(permissionCreate);

            if (!permissionRepository.CreatePermission(permissioMap))
            {
                return BadRequest("Error");
            }
            return Ok(permissioMap);
        }
    }
}
