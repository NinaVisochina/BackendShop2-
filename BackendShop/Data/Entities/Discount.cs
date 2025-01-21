namespace BackendShop.Data.Entities
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string Name { get; set; } // Назва знижки
        public decimal Percentage { get; set; } // Відсоток знижки
        public decimal? MinimumOrderAmount { get; set; } // Мінімальна сума замовлення
    }
}
