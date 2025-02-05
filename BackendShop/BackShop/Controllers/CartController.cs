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
    public async Task<IActionResult> Addtocart([FromBody] AddToCartRequest request)
    {
        Console.WriteLine($"Received userId: {request.UserId}, productId: {request.ProductId}, quantity: {request.Quantity}");
        if (string.IsNullOrEmpty(request.UserId))
        {
            return Unauthorized("User is not authenticated");
        }

        await _cartService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
        return Ok(new { message = "Item added to cart successfully" });
    }

    [HttpDelete("remove/{userId}/{productId}")]
    public async Task<IActionResult> RemoveFromCart(string userId, int productId)
    {
        await _cartService.RemoveFromCartAsync(userId, productId);
        return Ok();
    }

    [HttpPatch("update-quantity")]
    public async Task<IActionResult> UpdateCartItemQuantity([FromBody] AddToCartRequest request)
    {
        if (string.IsNullOrEmpty(request.UserId))
        {
            return Unauthorized("User is not authenticated");
        }

        await _cartService.UpdateCartItemQuantityAsync(request.UserId, request.ProductId, request.Quantity);
        return Ok();
    }

    [HttpDelete("clear/{userId}")]
    public async Task<IActionResult> ClearCart(string userId)
    {
        await _cartService.ClearCartAsync(userId);
        return Ok(new { message = "Cart has been cleared successfully" });
    }

}
