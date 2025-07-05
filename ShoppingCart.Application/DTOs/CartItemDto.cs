using System;

namespace ShoppingCart.Application.DTOs
{
    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        public CartItemDto() {}

    }
}
