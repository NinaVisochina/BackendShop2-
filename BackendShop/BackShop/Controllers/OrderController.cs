using BackendShop.Core.Dto.Order;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto orderDto)
    {
        var orderId = await _orderService.CreateOrderAsync(orderDto);
        return Ok(orderId);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserOrders(string userId)
    {
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return Ok(orders);
    }
}

