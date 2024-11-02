using GlideGear_backend.DbContexts;
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
                //if (createOrderDto.TransactionId == null && createOrderDto.OrderString == null)
                //{
                //    return false;
                //}

                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(u => u.UserId == userId);
                //decimal sum = cart.CartItems.Sum(s => s.Product.Price * s.Quantity);
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


                    //OrderString = createOrderDto.OrderString,
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
            }
            catch (DbUpdateException ex)
            {
                // Log the entire exception stack
                //Console.WriteLine(ex);
                //if (ex.InnerException != null)
                //{
                //    Console.WriteLine("Inner exception:");
                //    Console.WriteLine(ex.InnerException.Message);
                //}
                throw new Exception(ex.InnerException?.Message);
                //throw; // Optional: re-throw or handle as needed
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
                        ProductImage = item.Product.Img, // Use image URL directly
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                        OrderId = order.OrderString,
                        //OrderStatus = order.OrderStatus,
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
                    //OrderStatus = o.OrderStatus
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

        //public async Task<bool> UpdateOrderStatus(int orderId, UpdateOrderStatusDto value)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);

        //    if (order != null)
        //    {
        //        order.OrderStatus = value.OrderStatus;
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
