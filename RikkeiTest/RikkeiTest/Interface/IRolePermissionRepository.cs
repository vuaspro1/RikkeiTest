using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface IRolePermissionRepository
    {
        bool CreateRolePermission(RolePermission rolePermission);
        bool RolePermissionExists(int RoleId, int PermissionId);
    }
}
