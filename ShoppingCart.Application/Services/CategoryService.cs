using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;

namespace ShoppingCart.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var entity = new Category(dto.Name);
            await _repo.AddAsync(entity);
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(list);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateCategoryDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.UpdateName(dto.Name);
            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.SoftDeleteAsync(entity);
            return true;
        }
    }
}
