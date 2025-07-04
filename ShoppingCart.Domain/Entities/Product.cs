namespace ShoppingCart.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Price { get; private set; }
    public decimal? PromotionalPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Product() { } // EF

    public Product(string name, Guid categoryId, decimal price, decimal? promotionalPrice, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome obrigatório.", nameof(name));
        if (price <= 0) throw new ArgumentException("Preço deve ser > 0.", nameof(price));
        if (stockQuantity < 0) throw new ArgumentException("Estoque não pode ser negativo.", nameof(stockQuantity));

        Name = name;
        CategoryId = categoryId;
        Price = price;
        PromotionalPrice = promotionalPrice;
        StockQuantity = stockQuantity;
    }

    public void UpdateInfo(string name, Guid categoryId, decimal price, decimal? promotionalPrice)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nome obrigatório.", nameof(name));
        if (price <= 0) throw new ArgumentException("Preço deve ser > 0.", nameof(price));

        Name = name;
        CategoryId = categoryId;
        Price = price;
        PromotionalPrice = promotionalPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0) throw new ArgumentException("Estoque não pode ser negativo.", nameof(quantity));
        StockQuantity = quantity;
    }

    public void Deactivate() => IsActive = false;
}
