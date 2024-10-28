using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.WhishList_Model
{
    public class WhishList
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId  { get; set; }

        public virtual User? Users { get; set; }
        public virtual Product? Products { get; set; }

    }
}
