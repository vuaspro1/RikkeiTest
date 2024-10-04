using RikkeiTest.Dto;
using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        User Authenticate(string username, string password);
        ICollection<User> GetUsersToCheck();
        Task<UserView> GetUserById(int id);
        User GetUserToCheck(int id);
    }
}
