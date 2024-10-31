using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.UserDtos;
using GlideGear_backend.Models.User_Model.UserDtos;

namespace GlideGear_backend.Services.Users
{
    public interface IAuthServices
    {
        Task<bool> Register(UserRegistrationDto userRegistrationDto);
        Task<UserResponseDto> Login(LoginDto user);
    }
}
