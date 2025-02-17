using AutoMapper;
using BackendShop.Core.Dto.WishList;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly ShopDbContext _context;
        private readonly IWishListService _wishListService;        

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
            
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishList(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is required.");

            var wishList = await _wishListService.GetWishListAsync(userId);

            if (wishList == null)
                return NotFound("Wish list not found for the user.");

            return Ok(wishList);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishList([FromBody] WishListItemDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Отримуємо userId з токена
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (request == null || request.ProductId == 0)
            {
                return BadRequest("Invalid product ID.");
            }

            // Використовуємо лише ProductId з переданого DTO
            await _wishListService.AddToWishListAsync(userId, request.ProductId);
            return Ok(new { Message = "Product added to the wishlist" });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromWishList(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Отримуємо userId з токена
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            Console.WriteLine($"Attempting to remove product {productId} for user {userId}");

            var result = await _wishListService.RemoveFromWishListAsync(userId, productId);

            if (result)
                return Ok(new { Message = "Product removed from the wishlist" });

            return NotFound(new { Message = "Product not found in the wishlist" });
        }
    }
}