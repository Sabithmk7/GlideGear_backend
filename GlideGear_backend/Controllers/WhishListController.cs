using GlideGear_backend.Services.WhishListServices;
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

        public async Task<IActionResult> GetWhishLists()
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                {
                    return BadRequest("Invalid user ID.");
                }

                var res =await _whishListService.GetWhishList(userId);
                return Ok(res);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddOrRemove/{productId}")]
        public async Task<IActionResult> AddOrRemove(int productId)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                {
                    return BadRequest("Invalid user ID.");
                }
                string res=await _whishListService.AddOrRemove(userId,productId);
                return Ok(res);
                
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        

    }
}
