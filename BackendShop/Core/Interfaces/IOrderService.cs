using BackendShop.Core.Dto;
using BackendShop.Core.Dto.Order;
using BackendShop.Data.Entities;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDto orderDto);
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
    Task<Order?> GetOrderByIdAsync(int orderId);
    //Task<int> CreateOrderAsync(CheckoutDto model);
}

