using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.Order_Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int Quantity { get; set; }



        public virtual Order? Orders { get; set; }
        public virtual Product? Product { get; set; }
    }
}
