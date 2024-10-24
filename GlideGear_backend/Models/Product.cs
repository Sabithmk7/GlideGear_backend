using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]

        public decimal Price { get; set; }
        [Required]
        public string Img { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
