using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Core.Services
{
    public class WishListService : IWishListService
    {
        private readonly ShopDbContext _context;

        public WishListService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<WishListItem>> GetWishListAsync(string userId)
        {
            return await _context.WishListItems
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task AddToWishListAsync(string userId, int productId)
        {
            var existingItem = await _context.WishListItems
        .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (existingItem == null)
            {
                var newItem = new WishListItem
                {
                    UserId = userId,
                    ProductId = productId
                };

                _context.WishListItems.Add(newItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromWishListAsync(string userId, int productId)
        {
            var item = await _context.WishListItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (item != null)
            {
                _context.WishListItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
