using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.Dtos
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "Product title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Product description is required.")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
