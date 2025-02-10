﻿using AutoMapper;
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
        if (string.IsNullOrEmpty(userId))
            return BadRequest("UserId is required");

        var orders = await _orderService.GetUserOrdersAsync(userId);

        if (!orders.Any())
            return NotFound($"No orders found for user {userId}");

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

    [HttpGet("orders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        var ordersDto = _mapper.Map<List<OrderDto>>(orders);
        return Ok(ordersDto);
    }

    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto statusDto)
    {
        var success = await _orderService.UpdateOrderStatusAsync(orderId, statusDto.Status);

        if (!success) return NotFound("Order not found");

        return NoContent();
    }
}

