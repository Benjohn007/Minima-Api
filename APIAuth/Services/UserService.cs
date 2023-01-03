using APIAuth.Models;
using APIAuth.Repositories;

namespace APIAuth.Services
{
    public class UserService : IUserService
    {
        public User Get(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(x => x.UserName.Equals
            (userLogin.UserName,StringComparison.OrdinalIgnoreCase) && x.Password.Equals
            (userLogin.Password));
            return user;
        }
    }
}
