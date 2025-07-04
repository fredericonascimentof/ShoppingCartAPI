using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Infrastructure.Data;


namespace ShoppingCart.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ShoppingCartContext _ctx;

        public CartRepository(ShoppingCartContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Guid> CreateCartAsync() 
        {
            var cart = new Cart();
            await _ctx.Carts.AddAsync(cart);
            await _ctx.SaveChangesAsync();
            return cart.Id;
        }

        public async Task<Cart?> GetCartWithItemsAsync(Guid cartId) 
        {
            var cart = await _ctx.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);
            return cart;
        }

        public async Task AddOrUpdateItemAsync(Guid cartId, CartItem item) 
        {
            var cart = await _ctx.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new KeyNotFoundException("Carinho não encontrado!");

            var existing = cart.Items.FirstOrDefault(ci => ci.ProductId == item.ProductId);

            if (existing == null) 
            {
                cart.Items.Add(item);
                await _ctx.CartItems.AddAsync(item);
            }
            else 
            {
                existing.Quantity += item.Quantity;
                _ctx.CartItems.Update(existing);
            }
            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(Guid cartId, Guid itemId) 
        {
            var cartItem = await _ctx.CartItems.FindAsync(itemId);

            if (cartItem == null || cartItem.CartId != cartId)
                throw new KeyNotFoundException("Item não encontrado no carrinho!");

            _ctx.CartItems.Remove(cartItem);
            await _ctx.SaveChangesAsync();
        }
    }
}
