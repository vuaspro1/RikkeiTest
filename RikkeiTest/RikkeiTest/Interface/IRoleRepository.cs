using RikkeiTest.Dto;
using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface IRoleRepository
    {
        ICollection<RoleDto> GetRolesByUser(int id);
        bool CreateRole(Role role);
        Role GetRoleToCheck(int id);
    }
}
