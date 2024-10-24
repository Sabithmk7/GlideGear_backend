using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.ProductDtos;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.ProductServices
{
    public class ProductService:IProductServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(ApplicationDbContext context,IMapper mapper,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task AddProduct(ProductDto product, IFormFile image)
        {
            try
            {
                string? imageUrl = null;
                if (image != null && image.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Product", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imageUrl = "/Uploads/Product/" + fileName;
                }
                var prd = _mapper.Map<Product>(product);
                prd.Img = imageUrl;

                await _context.Products.AddAsync(prd);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
