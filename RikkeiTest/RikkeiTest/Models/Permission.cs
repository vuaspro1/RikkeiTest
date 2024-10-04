using System.ComponentModel.DataAnnotations;

namespace RikkeiTest.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
