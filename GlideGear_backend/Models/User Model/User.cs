using GlideGear_backend.Models.Order_Model;
using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [MaxLength(20,ErrorMessage ="Name should not execeed 20 character")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string? Password { get; set; }

        public string? Role {  get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual List<OrderMain> Orders { get; set; }
    }
}
