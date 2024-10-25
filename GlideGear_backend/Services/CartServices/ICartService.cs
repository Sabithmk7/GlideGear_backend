using GlideGear_backend.Models.Dtos;

namespace GlideGear_backend.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartViewDto>> GetCartItems(string token);
    }
}
