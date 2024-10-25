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
                        Title = p.Title,
                        Description = p.Description,
                        Price = p.Price,
                        ProductImage = HostUrl + p.Img,
                        Category = p.Category.Name
                    }
                    ).ToList();
                    return productWithCategory;

                }
                return new List<ProductViewDto>();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductViewDto> GetProductById(int id)
        {
            try
            {
                var prd = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
                if (prd == null)
                {
                    return new ProductViewDto();
                }
                else
                {
                    var p = new ProductViewDto
                    {
                        Id = prd.ProductId,
                        Title = prd.Title,
                        Description = prd.Description,
                        Price = prd.Price,
                        ProductImage = prd.Img,
                        Category = prd.Category.Name
                    };
                    return p;
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductViewDto>> GetProductByCategory(string categoryName)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryName).Select(p => new ProductViewDto 
                {
                    Id = p.ProductId,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    ProductImage = HostUrl + p.Img,
                    Category = p.Category.Name
                }).ToListAsync();

                return products;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var p = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if (p != null)
                {
                    _context.Products.Remove(p);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async Task UpdateProduct(int id, ProductDto productDto, IFormFile image)
        {
            try
            {
                var p = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                var categoryExist = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == productDto.CategoryId);
                if (categoryExist == null)
                {
                    throw new Exception("Category not found");
                }
                if (p != null)
                {
                    p.Title = productDto.Title;
                    p.Description = productDto.Description;
                    p.Price = productDto.Price;
                    p.CategoryId = productDto.CategoryId;

                    if(image !=null && image.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Product", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        p.Img= "/Uploads/Product/" + fileName;
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Product with ID: {id} not found!");
                }
            }catch(Exception ex){
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
        public async Task<List<ProductViewDto>> SearchProduct(string search)
        {
            var products = await _context.Products.Include(x=>x.Category).Where(p => p.Title.ToLower().Contains(search.ToLower())).ToListAsync();
            if (products != null)
            {
                return products.Select(s => new ProductViewDto
                {
                    Id=s.ProductId,
                    Title=s.Title,
                    Description=s.Description,
                    Price=s.Price,
                    ProductImage=HostUrl+s.Img,
                    Category=s.Category.Name
                }).ToList();
            }
            return new List<ProductViewDto>();
        }
        public async Task<List<ProductViewDto>> ProductPagination(int pagenumber=1,int size = 10)
        {
            try
            {
                var products = await _context.Products
                .Include(x => x.Category)
                .Skip((pagenumber - 1) * size)
                .Take(size)
                .ToListAsync();

                return products.Select(p => new ProductViewDto
                {
                    Id = p.ProductId,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    ProductImage = HostUrl + p.Img,
                    Category = p.Category.Name
                }).ToList();

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
