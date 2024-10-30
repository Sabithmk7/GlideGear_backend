namespace GlideGear_backend.Services.CloudinaryServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
