using BackendShop.Core.Dto.Cart;
using BackendShop.Core.Dto.Order;
using BackendShop.Data.Entities;

namespace BackendShop.Core.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity);
        Task RemoveFromCartAsync(string userId, int productId);
        Task ClearCartAsync(string userId);
        Task UpdateCartItemQuantityAsync(string userId, int productId, int quantity);
        //Task<CartDto> GetCartAsync(string userId);
        //Task AddToCartAsync(string userId, CartItemDto cartItemDto);
        //Task RemoveFromCartAsync(string userId, int productId);
        //Task ClearCartAsync(string userId);
        //Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto);
        //Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId);
    }
}
