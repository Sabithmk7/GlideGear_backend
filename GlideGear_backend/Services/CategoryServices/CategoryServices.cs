using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.CategoryDtos;

using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<List<CategoryDto>> getCategories()
        {
            var c = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryDto>>(c);
        }

        public async Task<bool> AddCategory(CategoryDto categoryDto)
        {
            var isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == categoryDto.Name.ToLower());
            if (!isExist)
            {
                var d = _mapper.Map<Category>(categoryDto);
                await _context.Categories.AddAsync(d);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<bool> RemoveCategory(int id)
        {
            var res = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            if (res == null)
            {
                return false;
            }
            else
            {
                _context.Categories.Remove(res);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
