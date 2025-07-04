using System;
using System.Collections.Generic;
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

namespace ShoppingCart.Tests.Services.CategoryTests
{
    public class CategoryServiceTests
    {
        private readonly IMapper _mapper;

        public CategoryServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateAsync_ValidDto_CreatesAndReturnsDto()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Category>()))
                    .Returns(Task.CompletedTask);
            var service = new CategoryService(mockRepo.Object, _mapper);
            var dto = new CreateCategoryDto("Eletrônicos");

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Eletrônicos", result.Name);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category("A"),
                new Category("B")
            };
            var mockRepo = new Mock<ICategoryRepository>();
            mockRepo.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(categories);
            var service = new CategoryService(mockRepo.Object, _mapper);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Collection(result,
                c => Assert.Equal("A", c.Name),
                c => Assert.Equal("B", c.Name));
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ReturnsFalse()
        {
            // Arrange
            var mockRepo = new Mock<ICategoryRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Category)null);
            var service = new CategoryService(mockRepo.Object, _mapper);

            // Act
            var success = await service.UpdateAsync(Guid.NewGuid(), new UpdateCategoryDto("X"));

            // Assert
            Assert.False(success);
        }

        [Fact]
        public async Task SoftDeleteAsync_Found_CallsSoftDeleteAndReturnsTrue()
        {
            // Arrange
            var cat = new Category("C");
            var mockRepo = new Mock<ICategoryRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(cat.Id))
                    .ReturnsAsync(cat);
            mockRepo.Setup(r => r.SoftDeleteAsync(cat))
                    .Returns(Task.CompletedTask);
            var service = new CategoryService(mockRepo.Object, _mapper);

            // Act
            var success = await service.SoftDeleteAsync(cat.Id);

            // Assert
            Assert.True(success);
            mockRepo.Verify(r => r.SoftDeleteAsync(cat), Times.Once);
        }
    }
}
