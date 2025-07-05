using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }
        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
        public Cart Cart { get; private set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; private set; }

        public CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantity));
            Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public CartItem() { }
    }

}
