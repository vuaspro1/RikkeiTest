using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly MyDbContext myDbContext;
        public RolePermissionRepository(MyDbContext myDbContext) 
        {
            this.myDbContext = myDbContext;
        }
        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool RolePermissionExists(int RoleId, int PermissionId)
        {
            return myDbContext.RolePermissions.Any(p => p.RoleId == RoleId && p.PermissionId == PermissionId);
        }

        public bool CreateRolePermission(RolePermission rolePermission)
        {
            myDbContext.Add(rolePermission);
            return Save();
        }
    }
}
