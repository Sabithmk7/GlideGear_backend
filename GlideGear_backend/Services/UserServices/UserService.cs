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
    }
}
