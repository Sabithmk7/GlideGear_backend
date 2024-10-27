using GlideGear_backend.Models.Dtos;
using GlideGear_backend.Services.CartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken = splitToken?[1];

                var res = await _cartService.GetCartItems(jwtToken);
                return Ok(res);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add/{productId}")]
        public async Task<IActionResult> Addtocart(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken = splitToken?[1];

                bool res=await _cartService.AddToCart(jwtToken, productId);
                if (res == true)
                {
                    return Ok("Product added to cart succesfully");
                }
                return BadRequest("Item already exist");
                
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveCart(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken = splitToken?[1];

                bool res = await _cartService.RemoveFromCart(jwtToken, productId);
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
        public async Task<IActionResult> IncreaseQty(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken = splitToken?[1];

                bool res=await _cartService.IncreaseQuantity(jwtToken, productId);
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
        public async Task<IActionResult> DecreaseQty(int productId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                var splitToken = token?.Split(' ');
                var jwtToken = splitToken?[1];

                bool res = await _cartService.DecreaseQuantity(jwtToken, productId);
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
    }
}
