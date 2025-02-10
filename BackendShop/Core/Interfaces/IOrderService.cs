using BackendShop.Core.Dto;
using BackendShop.Core.Dto.Order;
using BackendShop.Data.Entities;
using BackendShop.Data.Enums;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDto orderDto);
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
    Task<Order?> GetOrderByIdAsync(int orderId);    
    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);  // Новий метод для зміни статусу замовлення
    Task<IEnumerable<Order>> GetAllOrdersAsync();      // Метод для отримання всіх замовлень
}

