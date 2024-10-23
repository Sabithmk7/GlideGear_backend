using GlideGear_backend.Models.Dtos;
using GlideGear_backend.Services.Users;
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
    }
}
