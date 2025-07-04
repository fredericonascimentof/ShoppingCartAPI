using AutoMapper;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;

namespace ShoppingCart.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var prod = new Product(dto.Name, dto.CategoryId, dto.Price, dto.PromotionalPrice, dto.StockQuantity);
        await _repo.AddAsync(prod);
        return _mapper.Map<ProductDto>(prod);
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(Guid? categoryId = null)
    {
        var list = await _repo.GetAllAsync(categoryId);
        return _mapper.Map<IEnumerable<ProductDto>>(list);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var prod = await _repo.GetByIdAsync(id);
        return prod is null ? null : _mapper.Map<ProductDto>(prod);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var prod = await _repo.GetByIdAsync(id);
        if (prod is null) return false;
        prod.UpdateInfo(dto.Name, dto.CategoryId, dto.Price, dto.PromotionalPrice);
        prod.UpdateStock(dto.StockQuantity);
        await _repo.UpdateAsync(prod);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        var prod = await _repo.GetByIdAsync(id);
        if (prod is null) return false;
        await _repo.SoftDeleteAsync(prod);
        return true;
    }
}
