namespace GlideGear_backend.Models.Order_Model.Dtos
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCity { get; set; }
        public string HomeAddress { get; set; }
        public string OrderString { get; set; }
        public string TransactionId { get; set; }
    }
}
