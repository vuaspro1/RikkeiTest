using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MyDbContext myDbContext;
        private readonly IMapper mapper;

        public RoleRepository(MyDbContext myDbContext, IMapper mapper) 
        {
            this.myDbContext = myDbContext;
            this.mapper = mapper;
        }
        public bool CreateRole(Role role)
        {
            myDbContext.Add(role);
            return Save();
        }
        public ICollection<RoleDto> GetRolesByUser(int id)
        {
            var query = myDbContext.UserRoles.Where(item => item.UserId == id).Select(c => c.Role).ToList();
            return mapper.Map<List<RoleDto>>(query);
        }

        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Role GetRoleToCheck(int id)
        {
            var role = myDbContext.Roles.Where(item => item.Id == id).FirstOrDefault();
            return mapper.Map<Role>(role);
        }
    }
}
