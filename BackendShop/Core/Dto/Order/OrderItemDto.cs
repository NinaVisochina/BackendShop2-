namespace BackendShop.Core.Dto.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; } // Додано для назви продукту
    }
}
