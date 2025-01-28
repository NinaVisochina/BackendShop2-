using BackendShop.Core.Interfaces;
using BackendShop.Data.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        Console.WriteLine($"Received userId: {request.UserId}, productId: {request.ProductId}, quantity: {request.Quantity}");
        if (string.IsNullOrEmpty(request.UserId))
        {
            return Unauthorized("User is not authenticated");
        }

        await _cartService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
        return NoContent();
    }
}
