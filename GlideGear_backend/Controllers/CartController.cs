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
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var splitToken = token?.Split(' ');
            var jwtToken = splitToken?[1];

            var res = await _cartService.GetCartItems(jwtToken);
            return Ok(res);
        }
    }
}
