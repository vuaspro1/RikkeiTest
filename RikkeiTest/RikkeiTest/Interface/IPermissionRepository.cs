using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface IPermissionRepository
    {
        bool CreatePermission(Permission permission);
        Permission GetPermissionToCheck(int permissionId);
    }
}
