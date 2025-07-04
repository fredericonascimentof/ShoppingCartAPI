using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Infrastructure.Data;

namespace ShoppingCart.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ShoppingCartContext _ctx;
    public CategoryRepository(ShoppingCartContext ctx) => _ctx = ctx;

    public async Task AddAsync(Category category)
        => await _ctx.Categories.AddAsync(category);

    public async Task<Category?> GetByIdAsync(Guid id)
        => await _ctx.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _ctx.Categories.ToListAsync();

    public Task UpdateAsync(Category category)
    {
        _ctx.Categories.Update(category);
        return Task.CompletedTask;
    }

    public Task SoftDeleteAsync(Category category)
    {
        category.Deactivate();
        _ctx.Categories.Update(category);
        return Task.CompletedTask;
    }
}
