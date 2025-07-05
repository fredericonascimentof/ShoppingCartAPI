using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Moq.Language.Flow;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Application.Mapping;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;
using Xunit;

namespace ShoppingCart.Tests.Services.CartTests
{
    public class CartServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICartRepository> _mockCartRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly CartService _service;

        public CartServiceTests()
        {
            // Configura AutoMapper para testes
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();

            // Cria mocks dos repositórios
            _mockCartRepo = new Mock<ICartRepository>();
            _mockProductRepo = new Mock<IProductRepository>();

            // Instância o serviço com os mocks
            _service = new CartService(_mockCartRepo.Object, _mockProductRepo.Object, _mapper);
        }

        [Fact]
        public async Task CreateCartAsync_ReturnsGuid()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            _mockCartRepo.Setup(r => r.CreateCartAsync())
                         .ReturnsAsync(expectedId);

            // Act
            var result = await _service.CreateCartAsync();

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public async Task AddItemAsync_ProductNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange: produto inexistente
            _mockProductRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.AddItemAsync(Guid.NewGuid(), new AddCartItemDto(Guid.NewGuid(), 1)));
        }

        [Fact]
        public async Task AddItemAsync_InsufficientStock_ThrowsInvalidOperationException()
        {
            // Arrange: produto com estoque 1, mas solicitam 2
            var prod = new Product("P", Guid.NewGuid(), 10m, null, 1);
            _mockProductRepo.Setup(r => r.GetByIdAsync(prod.Id))
                            .ReturnsAsync(prod);
            _mockCartRepo.Setup(r => r.GetCartWithItemsAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(new Cart());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddItemAsync(Guid.NewGuid(), new AddCartItemDto(prod.Id, 2)));
        }

        [Fact]
        public async Task AddItemAsync_Valid_CallsRepoAndReturnsCartDto()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var prod = new Product("P", Guid.NewGuid(), 20m, 15m, 5);

            _mockProductRepo.Setup(r => r.GetByIdAsync(prod.Id))
                            .ReturnsAsync(prod);

            _mockCartRepo.Setup(r => r.AddOrUpdateItemAsync(cartId, It.IsAny<CartItem>()))
                         .Returns(Task.CompletedTask);

            var cart = new Cart(cartId);
            var item = new CartItem(cartId, prod.Id, 2, prod.PromotionalPrice.Value);
            cart.Items.Add(item);

            _mockCartRepo.Setup(r => r.GetCartWithItemsAsync(cartId))
                         .ReturnsAsync(cart);

            // Act
            var result = await _service.AddItemAsync(cartId, new AddCartItemDto(prod.Id, 2));

            // Assert
            _mockCartRepo.Verify(r => r.AddOrUpdateItemAsync(cartId,
                It.Is<CartItem>(ci => ci.Quantity == 2 && ci.UnitPrice == prod.PromotionalPrice)), Times.Once);

            Assert.Equal(cartId, result.CartId);
            Assert.Single(result.Items);
            Assert.Equal(2, result.Items.First().Quantity);
            Assert.Equal(prod.PromotionalPrice, result.Items.First().UnitPrice);
        }

        [Fact]
        public async Task GetCartAsync_NotFound_ThrowsKeyNotFoundException()
        {
            // Arrange: carrinho não existe
            _mockCartRepo.Setup(r => r.GetCartWithItemsAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetCartAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetCartAsync_Valid_ReturnsCartDto()
        {
            // Arrange: carrinho com um item
            var cartId = Guid.NewGuid();
            var cart = new Cart(cartId);
            cart.Items.Add(new CartItem(cartId, Guid.NewGuid(), 3, 5m));

            _mockCartRepo.Setup(r => r.GetCartWithItemsAsync(cartId))
                         .ReturnsAsync(cart);

            // Act
            var result = await _service.GetCartAsync(cartId);

            // Assert
            Assert.Equal(cartId, result.CartId);
            Assert.Single(result.Items);
            Assert.Equal(15m, result.Items.First().Subtotal);
        }
    }
}
