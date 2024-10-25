using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.ProductServices
{
    public class ProductService:IProductServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string HostUrl;
        public ProductService(ApplicationDbContext context,IMapper mapper,IWebHostEnvironment webHostEnvironment,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            HostUrl = configuration["HostUrl:url"];
        }

        public async Task<List<ProductViewDto>> GetProducts()
        {
            try
            {
                var p = await _context.Products.Include(x => x.Category).ToListAsync();
                if (p.Count > 0)
                {
                    var productWithCategory = p.Select(p => new ProductViewDto
                    {
                        Id = p.ProductId,
                        ProductName = p.Title,
                        ProductDescription = p.Description,
                        Price = p.Price,
                        ProductImage = HostUrl + p.Img,
                        Category = p.Category.Name
                    }
                    ).ToList();
                    return productWithCategory;

                }
                return [];
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
