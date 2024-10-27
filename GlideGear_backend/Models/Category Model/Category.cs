using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
