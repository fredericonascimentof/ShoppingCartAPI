using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.DTOs;

public record UpdateProductDto(
    string Name,
    Guid CategoryId,
    decimal Price,
    decimal? PromotionalPrice,
    int StockQuantity
);