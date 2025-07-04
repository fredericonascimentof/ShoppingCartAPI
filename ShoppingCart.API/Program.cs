using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Infrastructure.Repositories;
using ShoppingCart.Application.Services;
using ShoppingCart.Application.Mapping;
using ShoppingCart.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext
builder.Services.AddDbContext<ShoppingCartContext>(opts =>
    opts.UseSqlite("Data Source=shopping.db"));

// 2) Repositórios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// 3) Serviços
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();

// 4) AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 5) FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(cfg =>
        cfg.RegisterValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

// 6) Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
