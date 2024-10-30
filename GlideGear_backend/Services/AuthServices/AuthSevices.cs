using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public AuthSevices(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }


        public async Task<string> Register(UserRegistrationDto newUser)
        {
            try
            {
                var isUserExist = await _context.Users.SingleOrDefaultAsync(x => x.Email == newUser.Email);
                if (isUserExist != null)
                {
                    return "User already exists";
                }

                var hashPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                newUser.Password = hashPassword;

                var u = _mapper.Map<User>(newUser);
                _context.Users.Add(u);
                await _context.SaveChangesAsync();
                return "Registered Succesfully";
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
        public async Task<String> Login(LoginDto user)
        {
            var u = await _context.Users.SingleOrDefaultAsync(x => x.Email == user.Email);
            if (u == null || !ValidatePassword(user.Password, u.Password))
            {
                throw new InvalidOperationException("Invalid email or password");
            }
            if (u.isBlocked == true)
            {
                throw new Exception("You are blocked");
            }

            var uToken = GenerateToken(u);

            return uToken;
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
                expires: DateTime.UtcNow.AddHours(2)
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
