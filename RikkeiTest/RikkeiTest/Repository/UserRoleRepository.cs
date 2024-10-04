using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly MyDbContext myDbContext;
        public UserRoleRepository(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }
        public bool CreateUserRole(UserRole userRole) 
        {
            myDbContext.Add(userRole);
            return Save();
        }
        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool UserRoleExists(int UserId, int RoleId)
        {
            return myDbContext.UserRoles.Any(p => p.UserId == UserId && p.RoleId == RoleId);
        }
    }
}
