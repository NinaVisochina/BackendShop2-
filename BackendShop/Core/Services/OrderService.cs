using BackendShop.Core.Dto.Order;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using BackendShop.Data.Enums;
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
            DiscountId = orderDto.DiscountId > 0 ? orderDto.DiscountId : null,
            Address = orderDto.Address // Зберігаємо адресу
        };
        if (order.DiscountId != null)
        {
            var discountExists = await _context.Discounts.AnyAsync(d => d.DiscountId == order.DiscountId);
            if (!discountExists)
            {
                throw new Exception($"Discount with ID {order.DiscountId} does not exist.");
            }
        }
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.OrderId;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
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

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .ToListAsync();
    }

    public async Task UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto statusDto)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null) return; // Order not found

        // Перевіряємо, чи статус є дійсним значенням
        if (Enum.IsDefined(typeof(OrderStatus), statusDto.Status))
        {
            order.Status = (OrderStatus)statusDto.Status;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Invalid status value");
        }
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

}
