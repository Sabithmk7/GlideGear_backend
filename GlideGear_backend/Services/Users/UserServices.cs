using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.Users
{
    public class UserServices:IUserServices
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public UserServices(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                // Log the detailed exception
                throw new Exception($"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<User> Login(LoginDto user)
        {
            var u = await _context.Users.SingleOrDefaultAsync(x => x.Email == user.Email);
            return u;
        }
    }
}
