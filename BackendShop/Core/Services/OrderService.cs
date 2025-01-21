using BackendShop.Core.Dto.Order;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class OrderService : IOrderService
{
    private readonly ShopDbContext _context;

    public OrderService(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateOrderAsync(CreateOrderDto orderDto)
    {
        var order = new Order
        {
            UserId = orderDto.UserId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = orderDto.Items.Sum(i => i.Price * i.Quantity),
            Items = orderDto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList(),
            DiscountId = orderDto.DiscountId
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.OrderId;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }
}
