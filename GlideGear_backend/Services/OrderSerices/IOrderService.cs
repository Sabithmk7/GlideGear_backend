using GlideGear_backend.Models.Order_Model.Dtos;

namespace GlideGear_backend.Services.OrderSerices
{
    public interface IOrderService
    {
        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(PaymentDto payment);
        Task<bool> CreateOrder(string token,CreateOrderDto createOrderDto);
        Task<List<OrderViewDto>> GetOrderDetails(string token);
        Task<List<OrderAdminViewDto>> GetOrderDetailsAdmin();
        Task<decimal> TotalRevenue();
        Task<int> TotalProductsPurchased();

        Task<bool> UpdateOrderStatus(int orderId,UpdateOrderStatusDto value);

    }
}
