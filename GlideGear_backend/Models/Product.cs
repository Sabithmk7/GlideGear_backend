using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Product description is required.")]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Image URL is required.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? Img { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
