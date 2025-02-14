using BackendShop.Data.Entities;

namespace BackendShop.Core.Interfaces
{
    public interface IWishListService
    {
        Task<List<WishListItem>> GetWishListAsync(string userId);
        Task AddToWishListAsync(string userId, int productId);
        Task RemoveFromWishListAsync(string userId, int productId);
    }
}
