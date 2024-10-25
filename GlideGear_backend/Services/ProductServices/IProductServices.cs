using GlideGear_backend.Models.Dtos.ProductDtos;

namespace GlideGear_backend.Services.ProductServices
{
    public interface IProductServices
    {
        Task AddProduct(ProductDto product,IFormFile image);
        Task<List<ProductViewDto>> GetProducts();
        Task<ProductViewDto> GetProductById(int id);
        Task<List<ProductViewDto>> GetProductByCategory(string categoryName);
        Task<bool> DeleteProduct(int id);
        Task UpdateProduct(int id, ProductDto productDto, IFormFile image);
        Task<List<ProductViewDto>> SearchProduct(string search);
        Task<List<ProductViewDto>> ProductPagination(int pagenumber = 1, int size = 10);
    }
}
