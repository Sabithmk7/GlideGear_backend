﻿using GlideGear_backend.Models.Dtos.UserDtos;
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
        private readonly IAuthServices _userServices;
        public AuthController(IAuthServices userServices)
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
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto login)
        {
            try
            {
                var token =await _userServices.Login(login);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, 
                    Secure = true,    
                    SameSite = SameSiteMode.Strict, 
                    Expires = DateTime.UtcNow.AddHours(1) 
                };

                Response.Cookies.Append("AuthToken", token, cookieOptions);
                return Ok(new {Token=token});
            }catch(Exception ex)
            {
                return Unauthorized(new { Message = "Login failed", Error = ex.Message }); ;
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("AuthToken");
                return Ok("User Logged out");
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}