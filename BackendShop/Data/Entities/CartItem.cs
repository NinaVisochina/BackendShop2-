namespace BackendShop.Data.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductEntity Product { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
