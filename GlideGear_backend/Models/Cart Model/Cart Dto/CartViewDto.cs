﻿namespace GlideGear_backend.Models.Dtos
{
    public class CartViewDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ProductImage { get; set; }

        public decimal TotalAmount { get; set; }

        public int Quantity { get; set; }
    }
}
