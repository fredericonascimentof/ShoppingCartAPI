using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Application.Services;
using ShoppingCart.Infrastructure.Data;

namespace ShoppingCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _svc;
        private readonly ShoppingCartContext _ctx;

        public CategoriesController(CategoryService svc, ShoppingCartContext ctx)
        {
            _svc = svc;
            _ctx = ctx;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var categoryDto = await _svc.CreateAsync(dto);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetById),
                new { id = categoryDto.Id },
                categoryDto
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _svc.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cat = await _svc.GetByIdAsync(id);
            return cat is null ? NotFound() : Ok(cat);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto)
        {
            var ok = await _svc.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _svc.SoftDeleteAsync(id);
            if (!ok) return NotFound();
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
