using GlideGear_backend.Models.WhishList_Model.Dto;

namespace GlideGear_backend.Services.WhishListServices
{
    public interface IWhishListService
    {
        Task<bool> AddToWhishList(int userId,int productId);
        Task RemoveFromWhishList(int userId, int productId);
        Task<List<WhishListViewDto>> GetWhishList(int userId);

    }
}
