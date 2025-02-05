using AutoMapper;
using BackendShop.Core.Dto.Order;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    // Create order and return OrderDto
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto orderDto)
    {
        var orderId = await _orderService.CreateOrderAsync(orderDto);
        var order = await _orderService.GetOrderByIdAsync(orderId);

        if (order == null)
            return NotFound("Order not found");

        var orderDtoResult = _mapper.Map<OrderDto>(order);
        return Ok(orderDtoResult);
    }

    // Get all orders for a user
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserOrders(string userId)
    {
        var orders = await _orderService.GetUserOrdersAsync(userId);
        var ordersDto = _mapper.Map<List<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    // Get order details by ID
    [HttpGet("{orderId}/details")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null) return NotFound("Order not found");
        var orderDto = _mapper.Map<OrderDto>(order);
        return Ok(orderDto);
    }
}

