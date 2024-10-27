using GlideGear_backend.Models.Order_Model.Dtos;

namespace GlideGear_backend.Services.OrderSerices
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(string token,CreateOrderDto createOrderDto);
        Task<List<OrderViewDto>> GetOrderDetails(string token);

       

    }
}
