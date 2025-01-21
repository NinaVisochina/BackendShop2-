using BackendShop.Data.Entities;

namespace BackendShop.Core.Interfaces
{
    public interface IDiscountService
    {
        Task<Discount?> GetDiscountByIdAsync(int discountId);
        Task<IEnumerable<Discount>> GetAllDiscountsAsync();
    }
}
