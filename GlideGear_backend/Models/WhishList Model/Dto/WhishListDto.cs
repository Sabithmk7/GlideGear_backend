using System.ComponentModel.DataAnnotations;

namespace GlideGear_backend.Models.WhishList_Model.Dto
{
    public class WhishListDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
