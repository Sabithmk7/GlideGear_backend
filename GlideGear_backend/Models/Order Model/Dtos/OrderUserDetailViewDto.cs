using GlideGear_backend.Models.Dtos;

namespace GlideGear_backend.Models.Order_Model.Dtos
{
    public class OrderUserDetailViewDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerCity { get; set; }
        public string HomeAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string TransactionId { get; set; }
        public List<CartViewDto> OrderProducts { get; set; }
    }
}
