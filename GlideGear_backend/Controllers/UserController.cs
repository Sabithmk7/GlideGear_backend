using GlideGear_backend.ApiResponse;
using GlideGear_backend.Models.Dtos.UserDtos;
using GlideGear_backend.Models.User_Model.UserDtos;
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
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _userServices.GetUserById(userId);

                if (user == null)
                    return NotFound(new ApiResponses<string>(404, "User not found", null));

                var res = new ApiResponses<UserViewDto>(200, "Fetched user by id", user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An error occurred",null, ex.Message));
            }
        }


        [HttpPatch("blockorUnblock/{userId}")]
        public async Task<IActionResult> BlockOrUnblockUser(int userId)
        {
            try
            {
                var result = await _userServices.BlockAndUnblock(userId);
                var response = new ApiResponses<BlockUnblockResponse>(200, "Block/unblock action performed", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An error occurred", null, ex.Message));
            }
        }


    }
}
