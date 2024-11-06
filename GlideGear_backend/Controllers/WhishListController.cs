using GlideGear_backend.ApiResponse;
using GlideGear_backend.Models.WhishList_Model.Dto;
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

                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _whishListService.GetWhishList(userId);
                
                return Ok(new ApiResponses<List<WhishListViewDto>>(200,"Whishlist fetched correctly",res));
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500, "Failed to fetch wishlist",null, ex.Message));
            }
        }

        [HttpPost("AddOrRemove/{productId}")]
        [Authorize]
        public async Task<IActionResult> AddOrRemove(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                string res = await _whishListService.AddOrRemove(userId, productId);
                return Ok(new ApiResponses<string>(200,res));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500,"Operation on wishlist failed",null,ex.Message));
            }
        }
    }
}
