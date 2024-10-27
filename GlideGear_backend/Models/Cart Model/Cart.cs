using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }
    }
}
