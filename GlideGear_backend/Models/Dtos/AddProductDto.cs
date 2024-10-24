using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.Dtos
{
    public class AddProductDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
