namespace GlideGear_backend.Models.WhishList_Model.Dto
{
    public class WhishListViewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? ProductImage { get; set; }
    }
}
