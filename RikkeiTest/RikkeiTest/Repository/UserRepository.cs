using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace RikkeiTest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext myDbContext;
        private readonly IMapper mapper;
        public UserRepository(MyDbContext myDbContext, IMapper mapper) 
        {
            this.myDbContext = myDbContext;
            this.mapper = mapper;
        }

        public bool CreateUser( User user ) 
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                string passwordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                user.Password = passwordHash;
                myDbContext.Add(user);
            }
            return Save();
        }

        public User Authenticate(string username, string password)
        {
            using (SHA512 sha512 = SHA512.Create()) 
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                string passwordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();

                return myDbContext.Users.FirstOrDefault(user => user.UserName == username && user.Password == passwordHash);
            }
        }
        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<User> GetUsersToCheck()
        {
            var user = myDbContext.Users.ToList();
            return mapper.Map<List<User>>(user);
        }

        public async Task<UserView> GetUserById(int id) 
        {
            var user = myDbContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                                  .FirstOrDefault(u => u.Id == id);
            return new UserView
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Roles = user.UserRoles?.Select(ur => new RoleView
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                }).ToList()
            };
        }

        public User GetUserToCheck(int id)
        {
            var user = myDbContext.Users.Where(item => item.Id == id).FirstOrDefault();
            return mapper.Map<User>(user);
        }
    }
}
