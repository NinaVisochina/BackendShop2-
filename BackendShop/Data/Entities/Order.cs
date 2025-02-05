using BackendShop.Core.Dto.Product;
using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BackendShop.Data.Enums;

namespace BackendShop.Data.Entities
{
    [Table("tblOrders")]
    
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;  // Очікується обробка
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }
        // Додано поле для адреси
        public string Address { get; set; }

    }
}

   

