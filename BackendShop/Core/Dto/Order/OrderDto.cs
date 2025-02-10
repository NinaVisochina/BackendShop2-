namespace BackendShop.Core.Dto.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal TotalPrice { get; set; }
        public string Address { get; set; } // Додано поле для адреси
    }
}
