using System;
using System.Collections.Generic;

namespace ShoppingCart.Application.DTOs
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
