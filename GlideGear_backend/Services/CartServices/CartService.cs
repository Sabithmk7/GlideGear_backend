using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos;
using GlideGear_backend.Services.JwtService;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartService> _logger;

        public CartService(ApplicationDbContext context, ILogger<CartService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CartViewDto>> GetCartItems(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    throw new Exception("User id is null");
                }

                var user = await _context.Carts.Include(ci => ci.CartItems).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                //_logger.LogInformation($"{user}");

                if (user != null)
                {
                    return user.CartItems.Select(c => new CartViewDto
                    {
                        ProductId = c.ProductId,
                        ProductName = c.Product.Title,
                        Quantity = c.Quantity,
                        Price = c.Product.Price,
                        TotalAmount = c.Product.Price * c.Quantity,
                        ProductImage = c.Product.Img 
                    }).ToList();
                }
                return new List<CartViewDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddToCart(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart)
                                .ThenInclude(ci => ci.CartItems)
                                .ThenInclude(p => p.Product)
                                .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                    _context.Carts.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }

                var check = user.Cart?.CartItems?.FirstOrDefault(p => p.ProductId == productId);
                if (check != null)
                {
                    return false;
                }

                var item = new CartItem
                {
                    CartId = user.Cart.Id,
                    ProductId = productId,
                    Quantity = 1,
                };

                user?.Cart?.CartItems?.Add(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart)
                                    .ThenInclude(ci => ci.CartItems)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User is not found");
                }

                var deleteItem = user?.Cart?.CartItems?.FirstOrDefault(p => p.ProductId == productId);
                if (deleteItem == null)
                {
                    return false;
                }

                user.Cart.CartItems.Remove(deleteItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IncreaseQuantity(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart)
                                    .ThenInclude(ci => ci.CartItems)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);
                if (item == null)
                {
                    return false;
                }
                if (item.Quantity < 10)
                {
                    item.Quantity++;  
                    await _context.SaveChangesAsync();
                    return true;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DecreaseQuantity(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart)
                                    .ThenInclude(ci => ci.CartItems)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);
                if (item == null)
                {
                    return false;
                }
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    item.Quantity = 1;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
