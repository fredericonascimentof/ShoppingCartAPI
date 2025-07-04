using AutoMapper;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoppingCart.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<AddCartItemDto, CartItem>();
        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.Subtotal,
                       opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.Items,
                       opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Total,
                       opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity * i.UnitPrice)));

    }
}
