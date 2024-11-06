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
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var cart = await _cartService.GetCartItems(userId);
                if (cart.Count == 0)
                {
                    return Ok(new ApiResponses<IEnumerable<CartViewDto>>(200, "Cart is empty", cart));
                }
                return Ok(new ApiResponses<IEnumerable<CartViewDto>>(200, "Cart successfully fetched", cart));
            }catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error",null, ex.Message));
            }
        }

        [HttpPost("add/{productId}")]
        [Authorize]
        public async Task<IActionResult> Addtocart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                bool res=await _cartService.AddToCart(userId, productId);
                if (res == true)
                {
                    return Ok(new ApiResponses<bool>(200, "SuccessFully added", res));
                }
                return BadRequest(new ApiResponses<bool>(400, "Item already in cart", res));

            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }

        [HttpDelete("Delete/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                bool res = await _cartService.RemoveFromCart(userId, productId);
                if (res == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item is not found in cart",null, "Item is not found in cart"));
                }
                return Ok(new ApiResponses<string>(200, "Item successfully deleted"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }

        [HttpPut("IncreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult> IncreaseQty(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                bool res=await _cartService.IncreaseQuantity(userId, productId);
                if(res == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item not found in the cart",null, "Item not found in the cart"));
                }
                return Ok(new ApiResponses<string>(200, "Qty increased"));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }

        [HttpPut("DecreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQty(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                bool res = await _cartService.DecreaseQuantity(userId, productId);
                if (res == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item not found in the cart", null, "Item not found in the cart"));
                }
                return Ok(new ApiResponses<string>(200, "Qty decreased"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }

    }
}
