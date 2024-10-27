namespace GlideGear_backend.Models.Dtos.ProductDtos
{
    public class ProductViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ProductImage { get; set; }
    }
}
