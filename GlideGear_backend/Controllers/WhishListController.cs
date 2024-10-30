using GlideGear_backend.Services.WhishListServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhishListController : ControllerBase
    {
        private readonly IWhishListService _whishListService;
        public WhishListController(IWhishListService whishListService)
        {
            _whishListService = whishListService;
        }

        [HttpGet("GetWhishList")]
        [Authorize]
        public async Task<IActionResult> GetWhishLists()
        {
            try
            {

                int userId = GetUserIdFromClaims();
                var res = await _whishListService.GetWhishList(userId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddOrRemove/{productId}")]
        [Authorize]
        public async Task<IActionResult> AddOrRemove(int productId)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                string res = await _whishListService.AddOrRemove(userId, productId);
                return Ok(res);

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
