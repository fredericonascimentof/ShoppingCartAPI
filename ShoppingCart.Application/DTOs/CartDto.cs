using System;
using System.Collections.Generic;

namespace ShoppingCart.Application.DTOs
{
    public record CartDto(
        Guid CartId,
        IEnumerable<CartItemDto> Items,
        decimal Total
    );
}
