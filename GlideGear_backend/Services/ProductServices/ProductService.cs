using AutoMapper;
using GlideGear_backend.DbContexts;
using GlideGear_backend.Models;
using GlideGear_backend.Models.Dtos.ProductDtos;
using GlideGear_backend.Services.CloudinaryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Services.ProductServices
{
    public class ProductService : IProductServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(ApplicationDbContext context, IMapper mapper,  ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<ProductViewDto>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(x => x.Category).ToListAsync();
                if (products.Count > 0)
                {
                    var productWithCategory = products.Select(p => new ProductViewDto
                    {
                        Id = p.ProductId,
                        Title = p.Title,
                        Description = p.Description,
                        Price = p.Price,
                        ProductImage = p.Img, 
                        Category = p.Category.Name
                    }).ToList();
                    return productWithCategory;
                }
                return new List<ProductViewDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task AddProduct(ProductDto product, IFormFile image)
        {
            try
            {
                string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                var prd = _mapper.Map<Product>(product);
                prd.Img = imageUrl;

                await _context.Products.AddAsync(prd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
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
                return new ProductViewDto
                {
                    Id = prd.ProductId,
                    Title = prd.Title,
                    Description = prd.Description,
                    Price = prd.Price,
                    ProductImage = prd.Img,
                    Category = prd.Category.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductViewDto>> GetProductByCategory(string categoryName)
        {
            try
            {
                if (categoryName == "All")
                {
                    var prod = await _context.Products.Include(p => p.Category).ToListAsync();
                    var pr = prod.Select(p => new ProductViewDto
                    {
                        Id = p.ProductId,
                        Title = p.Title,
                        Description = p.Description,
                        ProductImage=p.Img,
                        Price = p.Price,
                        Category = p.Category.Name,
                    }).ToList();
                    return pr;
                }
                var products = await _context.Products.Include(p => p.Category)
                    .Where(p => p.Category.Name == categoryName)
                    .Select(p => new ProductViewDto
                    {
                        Id = p.ProductId,
                        Title = p.Title,
                        Description = p.Description,
                        Price = p.Price,
                        ProductImage = p.Img,
                        Category = p.Category.Name
                    }).ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateProduct(int id, ProductDto productDto, IFormFile image)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                var categoryExists = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == productDto.CategoryId);
                if (categoryExists == null)
                {
                    throw new Exception("Category not found");
                }
                if (product != null)
                {
                    product.Title = productDto.Title;
                    product.Description = productDto.Description;
                    product.Price = productDto.Price;
                    product.CategoryId = productDto.CategoryId;

                    if (image != null && image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                        product.Img = imageUrl;
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Product with ID: {id} not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<ProductViewDto>> SearchProduct(string search)
        {
            

            if (string.IsNullOrEmpty(search))
            { 
                 return new List<ProductViewDto>();
            }

           
            var products = await _context.Products.Include(x => x.Category)
                .Where(p => p.Title.ToLower().Contains(search.ToLower()))
                .ToListAsync();

            return products.Select(s => new ProductViewDto
            {
                Id = s.ProductId,
                Title = s.Title,
                Description = s.Description,
                Price = s.Price,
                ProductImage = s.Img,
                Category = s.Category.Name
            }).ToList();
        }

        public async Task<List<ProductViewDto>> ProductPagination(int pageNumber = 1, int size = 10)
        {
            try
            {
                var products = await _context.Products
                    .Include(x => x.Category)
                    .Skip((pageNumber - 1) * size)
                    .Take(size)
                    .ToListAsync();

                return products.Select(p => new ProductViewDto
                {
                    Id = p.ProductId,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    ProductImage = p.Img,
                    Category = p.Category.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
