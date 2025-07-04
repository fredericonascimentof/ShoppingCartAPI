using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Domain.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task UpdateAsync(Category category);
    Task SoftDeleteAsync(Category category);
}
