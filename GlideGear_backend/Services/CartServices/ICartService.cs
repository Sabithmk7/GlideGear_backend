using GlideGear_backend.ApiResponse;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos;

namespace GlideGear_backend.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartViewDto>> GetCartItems(int userId);
        Task<ApiResponses<CartItem>> AddToCart(int userId, int productId);
        Task<bool> RemoveFromCart(int userId, int productId);
        Task<ApiResponses<CartItem>> IncreaseQuantity(int userId, int productId);
        Task<bool> DecreaseQuantity(int userId, int productId);
    }
}
