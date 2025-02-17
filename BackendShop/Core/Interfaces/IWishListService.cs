using BackendShop.Core.Dto.WishList;
using BackendShop.Data.Entities;

namespace BackendShop.Core.Interfaces
{
    public interface IWishListService
    {
        Task<List<WishListItemDto>> GetWishListAsync(string userId);
        Task AddToWishListAsync(string userId, int productId);
        Task<bool> RemoveFromWishListAsync(string userId, int productId);
    }
}
