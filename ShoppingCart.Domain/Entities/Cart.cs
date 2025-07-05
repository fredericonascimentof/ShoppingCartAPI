using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public ICollection<CartItem> Items { get; private set; }
        public Cart()
        {
            Id = Guid.NewGuid();
            Items = new List<CartItem>();
        }

        public Cart(Guid id)
        {
            Id = id;
        }

        public void AddItem(CartItem item)
        {
            Items.Add(item);
        }
    }
}
