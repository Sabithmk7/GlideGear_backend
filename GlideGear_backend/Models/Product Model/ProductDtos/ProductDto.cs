using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.Dtos.ProductDtos
{
    public class ProductDto
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
