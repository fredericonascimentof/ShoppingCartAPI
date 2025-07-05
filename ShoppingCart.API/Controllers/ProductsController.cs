using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.DTOs;
using ShoppingCart.Application.Services;
using ShoppingCart.Infrastructure.Data;

namespace ShoppingCart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _svc;
    private readonly ShoppingCartContext _ctx;

    public ProductsController(ProductService svc, ShoppingCartContext ctx)
    {
        _svc = svc;
        _ctx = ctx;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var prod = await _svc.CreateAsync(dto);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = prod.Id }, prod);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? categoryId)
    {
        var list = await _svc.GetAllAsync(categoryId);
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var prod = await _svc.GetByIdAsync(id);
        return prod is null ? NotFound() : Ok(prod);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        var ok = await _svc.UpdateAsync(id, dto);
        if (!ok) return NotFound();
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _svc.SoftDeleteAsync(id);
        if (!ok) return NotFound();
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}

