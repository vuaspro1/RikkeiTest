using AutoMapper;
using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly MyDbContext myDbContext;
        private readonly IMapper mapper;
        public PermissionRepository(MyDbContext myDbContext, IMapper mapper)
        {
            this.myDbContext = myDbContext;
            this.mapper = mapper;
        }
        public bool CreatePermission(Permission permission)
        {
            myDbContext.Add(permission);
            return Save();
        }
        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
        public Permission GetPermissionToCheck(int id)
        {
            var permission = myDbContext.Permissions.Where(item => item.Id == id).FirstOrDefault();
            return mapper.Map<Permission>(permission);
        }
    }
}
