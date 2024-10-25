namespace GlideGear_backend.Services.JwtService
{
    public interface IJwtServices
    {
        int GetUserIdFromToken(string token);
    }
}
