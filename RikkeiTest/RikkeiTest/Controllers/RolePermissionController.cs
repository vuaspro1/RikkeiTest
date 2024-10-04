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
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionRepository rolePermissionRepository;
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        public RolePermissionController(IRolePermissionRepository rolePermissionRepository, IMapper mapper, IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            this.rolePermissionRepository = rolePermissionRepository;
            this.mapper = mapper;
            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
        }
        [HttpPost]
        public IActionResult CreateUserRole([FromBody] RolePermissionDto rolePermissionCreate)
        {
            if (rolePermissionCreate == null)
                return BadRequest();

            if (rolePermissionRepository.RolePermissionExists(rolePermissionCreate.RoleId, rolePermissionCreate.PermissionId))
                return BadRequest("RolePermission already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rolePermissionMap = mapper.Map<RolePermission>(rolePermissionCreate);
            rolePermissionMap.Permission = permissionRepository.GetPermissionToCheck(rolePermissionCreate.PermissionId);
            rolePermissionMap.Role = roleRepository.GetRoleToCheck(rolePermissionCreate.RoleId);

            if (!rolePermissionRepository.CreateRolePermission(rolePermissionMap))
            {
                return BadRequest("Error");
            }
            var createdUserRole = mapper.Map<RolePermissionDto>(rolePermissionMap);
            return Ok(createdUserRole);
        }
    }
}
