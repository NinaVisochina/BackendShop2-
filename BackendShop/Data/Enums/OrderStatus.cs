namespace BackendShop.Data.Enums
{
    public enum OrderStatus
    {
        Pending,   // Очікується обробка
        Processed, // Оброблено
        Delivered, // Доставлено
        Cancelled  // Скасовано
    }
}
