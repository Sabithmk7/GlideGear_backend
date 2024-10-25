using GlideGear_backend.DbContexts;
using GlideGear_backend.Models.Dtos;
using GlideGear_backend.Services.JwtService;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.CartServices
{
    public class CartService:ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtServices _jwtServices;
        private readonly string HostUrl;
        private readonly ILogger<CartService> _logger;
        public CartService(ApplicationDbContext context,IJwtServices jwtServices, IConfiguration configuration, ILogger<CartService> logger)
        {
            _context = context;
            _jwtServices = jwtServices;
            HostUrl = configuration["HostUrl:url"];
            _logger = logger;
        }

        public async Task<List<CartViewDto>> GetCartItems(string token)
        {
            try
            {
                int userId = _jwtServices.GetUserIdFromToken(token);
                if (userId == 0)
                {
                    throw new Exception("User id is null");
                }
                var user = await _context.Carts.Include(ci => ci.CartItems).ThenInclude(p => p.Product).FirstOrDefaultAsync(x=>x.Id==userId);
                _logger.LogInformation($"{user}");

                if(user != null)
                {
                    return user.CartItems.Select(c => new CartViewDto
                    {
                        ProductId = c.ProductId,
                        ProductName=c.Product.Title,
                        Quantity = c.Quantity,
                        Price=c.Product.Price,
                        TotalAmount=c.Product.Price*c.Quantity,
                        ProductImage=HostUrl+c.Product.Img
                    }).ToList();
                }
                return new List<CartViewDto>();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
