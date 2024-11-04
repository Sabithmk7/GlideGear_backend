using GlideGear_backend.Models;
using GlideGear_backend.Models.Order_Model.Dtos;

namespace GlideGear_backend.Services.OrderSerices
{
    public interface IOrderService
    {
        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(PaymentDto payment);
        Task<bool> CreateOrder(int userId, CreateOrderDto createOrderDto);
        Task<List<OrderViewDto>> GetOrderDetails(int userId);
        Task<List<OrderAdminViewDto>> GetOrderDetailsAdmin();
        Task<List<OrderUserDetailViewDto>> GetOrdersByUserId(int userId);
        Task<decimal> TotalRevenue();
        Task<int> TotalProductsPurchased();

        //Task<bool> UpdateOrderStatus(int orderId,UpdateOrderStatusDto value);

    }
}
