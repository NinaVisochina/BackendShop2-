using AutoMapper;
using BackendShop.Core.Dto.WishList;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Core.Services
{
    public class WishListService : IWishListService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;

        public WishListService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<WishListItemDto>> GetWishListAsync(string userId)
        {
            var wishList = await _context.WishListItems
                .Include(w => w.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            // Використовуйте AutoMapper для перетворення
            return _mapper.Map<List<WishListItemDto>>(wishList);
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

        public async Task<bool> RemoveFromWishListAsync(string userId, int productId)
        {
            var item = await _context.WishListItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (item != null)
            {
                _context.WishListItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Якщо елемента немає, повертається false
        }
    }
}