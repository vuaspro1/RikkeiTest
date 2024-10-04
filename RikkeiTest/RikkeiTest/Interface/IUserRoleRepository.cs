using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface IUserRoleRepository
    {
        bool CreateUserRole(UserRole userRole);
        bool UserRoleExists(int UserId, int RoleId);
    }
}
