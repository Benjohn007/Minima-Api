using APIAuth.Models;

namespace APIAuth.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
