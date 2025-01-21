namespace BackendShop.Core.Dto.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        //public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        //public decimal TotalPrice { get; set; }
    }
}
