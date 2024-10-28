using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models.WhishList_Model;
using GlideGear_backend.Models.WhishList_Model.Dto;
using GlideGear_backend.Services.JwtService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace GlideGear_backend.Services.WhishListServices
{
    public class WhishListService:IWhishListService
    {
        private readonly ApplicationDbContext _context;
     
        private readonly IConfiguration _configuration;
        private readonly string HostUrl;
        private readonly IMapper _mapper;
   

        public WhishListService(ApplicationDbContext context, IConfiguration configuration,IMapper mapper)
        {
            _context=context;
            _configuration = configuration;
            HostUrl = _configuration["HostUrl:url"];
            _mapper=mapper;
        }

        public async Task<string> AddOrRemove(int userId, int productId)
        {
            var isExist = await _context.WhishLists.Include(p=>p.Products).FirstOrDefaultAsync(w=>w.ProductId == productId && w.UserId==userId);
            if (isExist == null)
            {
                WhishListDto whishListDto = new WhishListDto()
                {
                    UserId = userId,
                    ProductId = productId,
                };

                var w = _mapper.Map<WhishList>(whishListDto);
                _context.WhishLists.Add(w);
                await _context.SaveChangesAsync();
                return "Item added to wishlist";
            }
            else
            {
                _context.WhishLists.Remove(isExist);
                await _context.SaveChangesAsync();
                return "Item removed from wishlist";
            }
            
        }

        //public async Task RemoveFromWhishList(int userId, int productId)
        //{
        //    try
        //    {
        //        var w = await _context.WhishLists.FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);
        //        if (w != null)
        //        {
        //            _context.WhishLists.Remove(w);
        //            await _context.SaveChangesAsync();
        //        }
        //    }catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<List<WhishListViewDto>> GetWhishList(int userId)
        {
            try
            {
                var items = await _context.WhishLists.Include(p => p.Products)
                .ThenInclude(c => c.Category)
                .Where(c => c.UserId == userId).ToListAsync();

                if (items != null)
                {
                    var p = items.Select(w => new WhishListViewDto
                    {
                        Id = w.Id,
                        ProductName = w.Products.Title,
                        ProductDescription = w.Products.Description,
                        Price = w.Products.Price,
                        ProductImage = HostUrl + w.Products.Img,
                        CategoryName = w.Products.Category.Name

                    }).ToList();

                    return p;
                }
                else
                {
                    return new List<WhishListViewDto>();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }
    }
}
