using GlideGear_backend.Models.Dtos.UserDtos;
using GlideGear_backend.Models.User_Model.UserDtos;

namespace GlideGear_backend.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserViewDto>> GetUsers();
        Task<BlockUnblockResponse> BlockAndUnblock(int userId);
        Task<UserViewDto> GetUserById(int userId);
        
    }
}
