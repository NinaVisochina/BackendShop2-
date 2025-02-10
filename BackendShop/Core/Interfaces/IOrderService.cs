using BackendShop.Core.Dto;
using BackendShop.Core.Dto.Order;
using BackendShop.Data.Entities;
using BackendShop.Data.Enums;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDto orderDto);
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
    Task<Order?> GetOrderByIdAsync(int orderId);
    //Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus); 
    // Замість UpdateOrderAsync можна використати метод, який приймає DTO для зміни статусу
    Task UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto statusDto);
    Task UpdateOrderAsync(Order order); // Для інших оновлень замовлень
    Task<IEnumerable<Order>> GetAllOrdersAsync();      // Метод для отримання всіх замовлень
}

