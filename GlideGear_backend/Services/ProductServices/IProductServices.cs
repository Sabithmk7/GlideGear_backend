using GlideGear_backend.Models.Dtos.ProductDtos;

namespace GlideGear_backend.Services.ProductServices
{
    public interface IProductServices
    {
        Task AddProduct(ProductDto product,IFormFile image);
    }
}
