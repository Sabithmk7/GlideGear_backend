using GlideGear_backend.Models.Dtos.CategoryDtos;

namespace GlideGear_backend.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<List<CategoryDto>> getCategories();
        Task<bool> AddCategory(CategoryDto category);
        Task<bool> RemoveCategory(int id);
    }
}
