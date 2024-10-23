using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos;

namespace GlideGear_backend.Services.Users
{
    public interface IUserServices
    {
        public Task<string> Register(UserRegistrationDto userRegistrationDto);
        public Task<User> Login()
    }
}
