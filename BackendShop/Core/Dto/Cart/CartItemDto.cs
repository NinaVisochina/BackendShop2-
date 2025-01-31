namespace BackendShop.Core.Dto.Cart
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new();
        //public int ProductId { get; set; }
        //public int Quantity { get; set; }
    }
}
