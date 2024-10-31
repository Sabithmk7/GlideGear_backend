namespace GlideGear_backend.Models.User_Model.UserDtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Error { get; set; }
    }
}
