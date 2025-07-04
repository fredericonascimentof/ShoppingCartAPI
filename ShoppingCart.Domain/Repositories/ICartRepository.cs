using ShoppingCart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<Guid> CreateCartAsync();
        Task<Cart?> GetCartWithItemsAsync(Guid cartId);
        Task AddOrUpdateItemAsync(Guid cartId, CartItem item);
        Task RemoveItemAsync(Guid cartId, Guid itemId);
    }
}
