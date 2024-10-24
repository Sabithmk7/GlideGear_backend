using GlideGear_backend.Models.Dtos.UserDtos;
using GlideGear_backend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [Authorize(Roles ="admin")]
        [HttpGet("getusers")]
       
        public async Task<IActionResult> getUsers()
        {
            try
            {
                var users = await _userServices.GetUsers();
                return Ok(users);
            }catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto newUser)
        {
            try
            {
                string isDone = await _userServices.Register(newUser);
                return Ok(isDone);
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message+" "+ex.InnerException?.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto login)
        {
            try
            {
                var token =await _userServices.Login(login);
                return Ok(new {Token=token});
            }catch(Exception ex)
            {
                return Unauthorized(new { Message = "Login failed", Error = ex.Message }); ;
            }
        }
    }
}
