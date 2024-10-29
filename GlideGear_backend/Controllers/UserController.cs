using GlideGear_backend.Services.Users;
using GlideGear_backend.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("getusers")]

        public async Task<IActionResult> getUsers()
        {
            try
            {
                var users = await _userServices.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPatch("blockorUnblock/{userId}")]
        public async Task<IActionResult> BlockOrUnblockUser(int userId)
        {
            try
            {
                string res = await _userServices.BlockAndUnblock(userId);
                return Ok(res);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
