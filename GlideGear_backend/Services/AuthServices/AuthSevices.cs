using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.UserDtos;
using GlideGear_backend.Models.User_Model.UserDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Razorpay.Api;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GlideGear_backend.Services.Users
{
    public class AuthSevices : IAuthServices
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthSevices> _logger;
        public AuthSevices(ApplicationDbContext context, IMapper mapper, IConfiguration configuration, ILogger<AuthSevices> logger)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<bool> Register(UserRegistrationDto newUser)
        {
            try
            {
                var isUserExist = await _context.Users.SingleOrDefaultAsync(x => x.Email == newUser.Email);
                if (isUserExist != null)
                {
                    return false;
                }

                var hashPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                newUser.Password = hashPassword;

                var u = _mapper.Map<User>(newUser);
                _context.Users.Add(u);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<UserResponseDto> Login(LoginDto user)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user...");

                var u = await _context.Users.SingleOrDefaultAsync(x => x.Email == user.Email);
                if (u == null)
                {
                    _logger.LogWarning("User not found.");
                    return new UserResponseDto { Error = "Not Found" };
                }

                _logger.LogInformation("User found. Validating password...");
                var pass = ValidatePassword(user.Password, u.Password);

                if (!pass)
                {
                    _logger.LogWarning("Invalid password.");
                    return new UserResponseDto { Error = "Invalid password" };
                }

                if (u.isBlocked==true)
                {
                    _logger.LogWarning("User is blocked.");
                    return new UserResponseDto { Error = "User Blocked" };
                }

                _logger.LogInformation("Generating token...");
                var uToken = GenerateToken(u);

                return new UserResponseDto
                {
                    Token = uToken,
                    Role = u.Role,
                    Email = u.Email,
                    Id = u.Id,
                    Name = u.UserName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Login method: {ex.Message}");
                throw;
            }
        }




        //Token Generation

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName ),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var token = new JwtSecurityToken(
                claims: claim,
                signingCredentials: credentails,
                expires: DateTime.UtcNow.AddDays(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //Bcrypt password verification
        private bool ValidatePassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
