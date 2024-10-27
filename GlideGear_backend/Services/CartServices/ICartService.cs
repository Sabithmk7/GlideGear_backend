using GlideGear_backend.Models.Dtos;

namespace GlideGear_backend.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartViewDto>> GetCartItems(string token);
        Task<bool> AddToCart(string token, int productId);
        Task<bool> RemoveFromCart(string token, int productId);
        Task<bool> IncreaseQuantity(string token, int productId);
        Task<bool> DecreaseQuantity(string token, int productId);
    }
}
