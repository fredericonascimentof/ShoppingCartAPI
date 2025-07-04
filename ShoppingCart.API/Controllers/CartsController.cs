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
    public class CartsController : ControllerBase 
    {
        private readonly CartService _cartService;
        private readonly ShoppingCartContext _ctx;

        public CartsController(CartService cartService, ShoppingCartContext ctx)
        {
            _cartService = cartService;
            _ctx = ctx;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCart() 
        {
            var cartId = await _cartService.CreateCartAsync();
            return CreatedAtAction(nameof(GetCartItems), new { cartId }, cartId);
        }

        [HttpPost("{cartId:guid}/items")]
        public async Task<IActionResult> AddItem(Guid cartId, [FromBody] AddCartItemDto dto)
        {
            try
            {
               
                var cartDto = await _cartService.AddItemAsync(cartId, dto);
                return Ok(cartDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{cartId:guid}/items")]
        public async Task<IActionResult> GetCartItems(Guid cartId)
        {
            try
            {
                var cartDto = await _cartService.GetCartAsync(cartId);
                return Ok(cartDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


    }

}
