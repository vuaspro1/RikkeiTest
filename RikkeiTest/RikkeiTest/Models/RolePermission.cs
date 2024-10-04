using Microsoft.EntityFrameworkCore;

namespace RikkeiTest.Models
{
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
