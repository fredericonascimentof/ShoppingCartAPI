using System;
using System.Collections.Generic;
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

namespace ShoppingCart.Tests.Services.ProductTests
{
    public class ProductServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductRepository> _mockRepo;

        public ProductServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
            _mockRepo = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task CreateAsync_ValidDto_CreatesAndReturnsDto()
        {
            // Arrange
            var dto = new CreateProductDto("Test", Guid.NewGuid(), 100m, null, 5);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>()))
                     .Returns(Task.CompletedTask);
            var service = new ProductService(_mockRepo.Object, _mapper);

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
            Assert.Equal(100m, result.Price);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_NoFilter_ReturnsAll()
        {
            // Arrange
            var list = new List<Product>
            {
                new Product("A", Guid.NewGuid(), 10m, null, 1),
                new Product("B", Guid.NewGuid(), 20m, null, 2)
            };
            _mockRepo.Setup(r => r.GetAllAsync(null))
                     .ReturnsAsync(list);
            var service = new ProductService(_mockRepo.Object, _mapper);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "A");
            Assert.Contains(result, p => p.Name == "B");
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Product)null);
            var service = new ProductService(_mockRepo.Object, _mapper);
            var dto = new UpdateProductDto("X", Guid.NewGuid(), 50m, null, 3);

            // Act
            var success = await service.UpdateAsync(Guid.NewGuid(), dto);

            // Assert
            Assert.False(success);
        }

        [Fact]
        public async Task UpdateAsync_Found_UpdatesAndReturnsTrue()
        {
            // Arrange
            var prod = new Product("Old", Guid.NewGuid(), 30m, null, 2);
            _mockRepo.Setup(r => r.GetByIdAsync(prod.Id))
                     .ReturnsAsync(prod);
            var service = new ProductService(_mockRepo.Object, _mapper);
            var dto = new UpdateProductDto("New", prod.CategoryId, 40m, 35m, 10);

            // Act
            var success = await service.UpdateAsync(prod.Id, dto);

            // Assert
            Assert.True(success);
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Product>(p =>
                p.Name == "New" &&
                p.Price == 40m &&
                p.PromotionalPrice == 35m &&
                p.StockQuantity == 10
            )), Times.Once);
        }

        [Fact]
        public async Task SoftDeleteAsync_Found_CallsSoftDeleteAndReturnsTrue()
        {
            // Arrange
            var prod = new Product("P", Guid.NewGuid(), 10m, null, 1);
            _mockRepo.Setup(r => r.GetByIdAsync(prod.Id))
                     .ReturnsAsync(prod);
            _mockRepo.Setup(r => r.SoftDeleteAsync(prod))
                     .Returns(Task.CompletedTask);
            var service = new ProductService(_mockRepo.Object, _mapper);

            // Act
            var success = await service.SoftDeleteAsync(prod.Id);

            // Assert
            Assert.True(success);
            _mockRepo.Verify(r => r.SoftDeleteAsync(prod), Times.Once);
        }
    }
}
