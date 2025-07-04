using System;

namespace ShoppingCart.Application.DTOs
{
    public record AddCartItemDto(Guid ProductId, int Quantity);
}
