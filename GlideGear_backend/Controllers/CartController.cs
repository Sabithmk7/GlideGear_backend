using GlideGear_backend.ApiResponse;
using GlideGear_backend.Models.Dtos;
using GlideGear_backend.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                int userId = GetUserIdFromClaims();

                var cart = await _cartService.GetCartItems(userId);
                if (cart.Count == 0)
                {
                    return Ok(new ApiResponses<IEnumerable<CartViewDto>>(200, "Cart is empty", cart));
                }
                return Ok(new ApiResponses<IEnumerable<CartViewDto>>(200, "Cart successfully fetched", cart));
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add/{productId}")]
        [Authorize]
        public async Task<IActionResult> Addtocart(int productId)
        {
            try
            {
                int userId = GetUserIdFromClaims();

                bool res=await _cartService.AddToCart(userId, productId);
                if (res == true)
                {
                    return Ok(new ApiResponses<bool>(200, "SuccessFully added", res));
                }
                return BadRequest(new ApiResponses<bool>(400, "Item already in cart", res));

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCart(int productId)
        {
            try
            {
                int userId = GetUserIdFromClaims();

                bool res = await _cartService.RemoveFromCart(userId, productId);
                if (res == false)
                {
                    return BadRequest("Item is not found in cart");
                }
                return Ok("Item successfully deleted");
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("IncreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult> IncreaseQty(int productId)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                bool res=await _cartService.IncreaseQuantity(userId, productId);
                if(res == false)
                {
                    return BadRequest("Item not found in the cart");
                }
                return Ok("Qty increased");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("DecreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQty(int productId)
        {
            try
            {
                int userId = GetUserIdFromClaims();

                bool res = await _cartService.DecreaseQuantity(userId, productId);
                if (res == false)
                {
                    return BadRequest("Item not found in the cart");
                }
                return Ok("Qty decreased");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private int GetUserIdFromClaims()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            throw new Exception("Invalid user ID.");
        }
    }
}
