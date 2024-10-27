namespace GlideGear_backend.Models.Order_Model.Dtos
{
    public class PaymentDto
    {
        public string? razorpay_payment_id { get; set; }
        public string? razorpay_order_id { get; set; }
        public string? razorpay_signature { get; set; }
    }
}
