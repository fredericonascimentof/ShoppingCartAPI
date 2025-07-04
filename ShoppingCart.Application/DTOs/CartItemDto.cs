using System;

namespace ShoppingCart.Application.DTOs
{
    public record CartItemDto(
        Guid ItemId,
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );
}
