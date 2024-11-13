using GlideGear_backend.DbContexts;
using GlideGear_backend.Models.Dtos;
using GlideGear_backend.Models.Order_Model;
using GlideGear_backend.Models.Order_Model.Dtos;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace GlideGear_backend.Services.OrderSerices
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderService(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> RazorOrderCreate(long price)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            Random random = new Random();
            string TrasactionId = random.Next(0, 1000).ToString();
            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TrasactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);
            Razorpay.Api.Order order = client.Order.Create(input);
            var OrderId = order["id"].ToString();

            return OrderId;
        }

        public bool RazorPayment(PaymentDto payment)
        {
            if (payment == null ||
                string.IsNullOrEmpty(payment.razorpay_payment_id) ||
                string.IsNullOrEmpty(payment.razorpay_order_id) ||
                string.IsNullOrEmpty(payment.razorpay_signature))
            {
                return false;
            }

            try
            {
                RazorpayClient client = new RazorpayClient(
                    _configuration["Razorpay:KeyId"],
                    _configuration["Razorpay:KeySecret"]
                );

                Dictionary<string, string> attributes = new Dictionary<string, string>
                {
                    { "razorpay_payment_id", payment.razorpay_payment_id },
                    { "razorpay_order_id", payment.razorpay_order_id },
                    { "razorpay_signature", payment.razorpay_signature }
                };

                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateOrder(int userId, CreateOrderDto createOrderDto)
        {
            try
            {
                var cart = await _context.Carts?.Include(c => c.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.UserId == userId);
                if (cart == null)
                {
                    throw new Exception("Cart is empty");
                }
                var order = new OrderMain
                {
                    userId = userId,
                    OrderDate = DateTime.Now,
                    CustomerName = createOrderDto.CustomerName,
                    CustomerEmail = createOrderDto.CustomerEmail,
                    CustomerCity = createOrderDto.CustomerCity,
                    CustomerPhone = createOrderDto.CustomerPhone,
                    HomeAddress = createOrderDto.HomeAddress,
                    Total=createOrderDto.Total,
                    OrderString=createOrderDto.OrderString,
                    TransactionId=createOrderDto.TransactionId,
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        TotalPrice = ci.Quantity * ci.Product.Price
                    }).ToList()
                };

                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == cartItem.ProductId);
                    if (product != null)
                        if (product.Stock < cartItem.Quantity)
                        {
                            return false;
                        }

                    product.Stock -= cartItem.Quantity;

                }

                await _context.Orders.AddAsync(order);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
           
                throw new Exception(ex.InnerException?.Message);
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderViewDto>> GetOrderDetails(int userId)
        {
            var orders = await _context.Orders.Include(oi => oi.OrderItems).ThenInclude(p => p.Product).Where(u => u.userId == userId).ToListAsync();

            var OrderDetails = new List<OrderViewDto>();

            foreach (var order in orders)
            {
                foreach (var item in order.OrderItems)
                {
                    var OrderDetail = new OrderViewDto
                    {
                        Id = item.ProductId,
                        OrderDate = order.OrderDate,
                        ProductName = item.Product.Title,
                        ProductImage = item.Product.Img, 
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                        OrderId = order.OrderString,
 
                    };
                    OrderDetails.Add(OrderDetail);
                }
            }
            return OrderDetails;
        }

        public async Task<List<OrderAdminViewDto>> GetOrderDetailsAdmin()
        {
            var orders = await _context.Orders.Include(oi => oi.OrderItems).ToListAsync();

            if (orders.Count != 0)
            {
                var orderDetails = orders.Select(o => new OrderAdminViewDto
                {
                    Id = o.Id,
                    CustomerEmail = o.CustomerEmail,
                    CustomerName = o.CustomerName,
                    OrderId = o.OrderString,
                    TransactionId = o.TransactionId,
                    OrderDate = o.OrderDate,
                }).ToList();
                return orderDetails;
            }
            return new List<OrderAdminViewDto>();
        }

        public async Task<decimal> TotalRevenue()
        {
            try
            {
                var total = await _context.OrderItems.SumAsync(itm => itm.TotalPrice);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> TotalProductsPurchased()
        {
            try
            {
                var totalP = await _context.OrderItems.SumAsync(i => i.Quantity);
                return totalP;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OrderUserDetailViewDto>> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.userId == userId)
                    .ToListAsync();

           
                if (orders == null || !orders.Any())
                    return new List<OrderUserDetailViewDto>();

                var orderDetails = orders.Select(orders => new OrderUserDetailViewDto
                {
                    Id = orders.Id,
                    CustomerEmail = orders.CustomerEmail,
                    CustomerName = orders.CustomerName,
                    CustomerCity = orders.CustomerCity,
                    CustomerPhone = orders.CustomerPhone,
                    OrderId = orders.OrderString,
                    HomeAddress = orders.HomeAddress,
                    TransactionId = orders.TransactionId,
                    OrderDate = orders.OrderDate,
                    OrderProducts = orders.OrderItems.Select(oi => new CartViewDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Title,
                        Price = oi.Product.Price,
                        Quantity = oi.Quantity,
                        TotalAmount = oi.TotalPrice,
                        ProductImage = oi.Product.Img
                    }).ToList()
                }).ToList();

                return orderDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
