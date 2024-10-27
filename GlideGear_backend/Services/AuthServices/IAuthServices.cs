using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.UserDtos;

namespace GlideGear_backend.Services.Users
{
    public interface IAuthServices
    {
        Task<string> Register(UserRegistrationDto userRegistrationDto);
        Task<string> Login(LoginDto user);
    }
}
