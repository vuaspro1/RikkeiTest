using System.ComponentModel.DataAnnotations;

namespace RikkeiTest.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
