using RikkeiTest.Dto;

namespace RikkeiTest.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public ICollection<RoleDto>? Roles { get; set; }
    }
}
