using GlideGear_backend.DbContexts;
using GlideGear_backend.Models.Order_Model;
using GlideGear_backend.Models.Order_Model.Dtos;
using GlideGear_backend.Services.JwtService;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.OrderSerices
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IJwtServices _jwtServices;
        private readonly string HostUrl;
        public OrderService(ApplicationDbContext context, IConfiguration configuration, IJwtServices jwtServices)
        {
            _configuration = configuration;
            _context = context;
            _jwtServices = jwtServices;
            HostUrl = _configuration["Host:Url"];
        }
        public async Task<bool> CreateOrder(string token, CreateOrderDto createOrderDto)
        {
            try
            {
                int userId = _jwtServices.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("User not found");
                }
                if(createOrderDto.TransactionId==null && createOrderDto.OrderString == null)
                {
                    return false;
                }

                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.UserId == userId);
                var order = new Order
                {
                    userId = userId,
                    OrderDate = DateTime.Now,
                    CustomerName = createOrderDto.CustomerName,
                    CustomerEmail = createOrderDto.CustomerEmail,
                    CustomerCity = createOrderDto.CustomerCity,
                    CustomerPhone = createOrderDto.CustomerPhone,
                    HomeAddress = createOrderDto.HomeAddress,
                    OrderString = createOrderDto.OrderString,
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Quantity * ci.Product.Price
                    }).ToList()
                };
                await _context.Orders.AddAsync(order);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
                
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderViewDto>> GetOrderDetails(string token)
        {
            var userId = _jwtServices.GetUserIdFromToken(token);
            if (userId == 0)
            {
                throw new Exception("User is not valid");
            }

            var orders=await _context.Orders.Include(oi=>oi.OrderItems).ThenInclude(p=>p.Product).Where(u=>u.userId == userId).ToListAsync();

            var OrderDetails=new List<OrderViewDto>();

            foreach (var order in orders)
            {
                foreach (var item in order.OrderItems)
                {
                    var OrderDetail = new OrderViewDto
                    {
                        Id = item.ProductId,
                        OrderDate = order.OrderDate,
                        ProductName = item.Product.Title,
                        ProductImage = HostUrl + item.Product.Img,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                        OrderId = order.OrderString,
                        OrderStatus = order.OrderStatus,
                    };
                    OrderDetails.Add(OrderDetail);
                }
            }
            return OrderDetails;
        }
    }
}
