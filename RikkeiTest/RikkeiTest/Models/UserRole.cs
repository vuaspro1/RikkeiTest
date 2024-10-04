using Microsoft.EntityFrameworkCore;

namespace RikkeiTest.Models
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
