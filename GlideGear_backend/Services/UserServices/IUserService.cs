using GlideGear_backend.Models.Dtos.UserDtos;

namespace GlideGear_backend.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserViewDto>> GetUsers();
    }
}
