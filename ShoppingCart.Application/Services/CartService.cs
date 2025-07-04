using System;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Application.Mapping;

namespace ShoppingCart.Application.Services;

public class CartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;
    public CartService(ICartRepository cartRepo, IProductRepository productRepo, IMapper mapper) 
    {
        _cartRepo = cartRepo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<Guid> CreateCartAsync()
    {
        var cartId = await _cartRepo.CreateCartAsync();
        return cartId;
    }

    public async Task<CartDto> AddItemAsync(Guid cartId, AddCartItemDto dto) 
    {
        var product = await _productRepo.GetByIdAsync(dto.ProductId);

        if (product == null)
            throw new KeyNotFoundException("Produto não encontrado.");

        if (!product.IsActive)
            throw new InvalidOperationException("Produto inativo.");

        var unitPrice = product.PromotionalPrice.HasValue && product.PromotionalPrice.Value > 0
            ? product.PromotionalPrice.Value : product.Price;

        var cartItem = _mapper.Map<CartItem>(dto);
        cartItem = new CartItem(cartId, dto.ProductId, dto.Quantity, unitPrice);

        await _cartRepo.AddOrUpdateItemAsync(cartId, cartItem);

        var cart = await _cartRepo.GetCartWithItemsAsync(cartId);

        if (cart == null)
            throw new KeyNotFoundException("Carrinho não encontrado.");

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> GetCartAsync(Guid cartId)
    {
        var cart = await _cartRepo.GetCartWithItemsAsync(cartId);

        if (cart == null)
            throw new KeyNotFoundException("Carrinho não encontrado.");

        return _mapper.Map<CartDto>(cart);
    }


}

