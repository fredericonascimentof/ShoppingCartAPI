using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Infrastructure.Data;

namespace ShoppingCart.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShoppingCartContext _ctx;
    public ProductRepository(ShoppingCartContext ctx) => _ctx = ctx;

    public async Task AddAsync(Product product)
        => await _ctx.Products.AddAsync(product);

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _ctx.Products.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Product>> GetAllAsync(Guid? categoryId = null)
    {
        var query = _ctx.Products.AsQueryable();
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);
        return await query.ToListAsync();
    }

    public Task UpdateAsync(Product product)
    {
        _ctx.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task SoftDeleteAsync(Product product)
    {
        product.Deactivate();
        _ctx.Products.Update(product);
        return Task.CompletedTask;
    }
}
