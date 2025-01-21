namespace BackendShop.Core.Dto.Order
{
    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public int? DiscountId { get; set; }
        //public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
