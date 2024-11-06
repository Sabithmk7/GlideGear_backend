using Azure;
using GlideGear_backend.ApiResponse;
using GlideGear_backend.Models.Dtos.UserDtos;
using GlideGear_backend.Models.User_Model.UserDtos;
using GlideGear_backend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthServices authServices, ILogger<AuthController> logger)
        {
            _authServices = authServices;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto newUser)
        {
            try
            {
                bool isDone = await _authServices.Register(newUser);
                if (isDone == false)
                {
                    var res = new ApiResponses<string>(409, "User already exists");
                    return Conflict(res);
                }

                return Ok(new ApiResponses<bool>(200, "User registered succesfully", isDone));
            }
            catch (Exception ex)
            {
                var res = new ApiResponses<string>(500, "Server error", null, ex.Message);
                return StatusCode(500, res);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto login)
        {
            try
            {
                var res = await _authServices.Login(login);
                _logger.LogInformation($" result is {res}");
                if (res.Error == "Not Found")
                {
                    return NotFound(new ApiResponses<string>(404, "NotFound", null, "Please SignUp, user not found"));
                }
                if (res.Error == "User Blocked")
                {
                    return StatusCode(403, new ApiResponses<string>(403, "Forbiden", null, "User is blocked by admin"));
                }
                if(res.Error == "Invalid password")
                {
                    return BadRequest(new ApiResponses<string>(400, "BadRequest", null, res.Error));
                }

                return Ok(new ApiResponses<UserResponseDto>(200, "Login successfully", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server issue", null, ex.Message)); ;
            }
        }
    }
}
