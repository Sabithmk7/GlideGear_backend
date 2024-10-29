using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models.Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<UserViewDto>> GetUsers()
        {
            try
            {
                var u = await _context.Users.ToListAsync();
                return _mapper.Map<List<UserViewDto>>(u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> BlockAndUnblock(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                user.isBlocked = !user.isBlocked;
                await _context.SaveChangesAsync();

                return user.isBlocked==true ? "User is blocked" : "User is unblocked";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
