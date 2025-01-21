using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Data.Entities
{
    public class User:IdentityUser

    {
        [StringLength(100)]
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Birthdate { get; set; }

        // Navigation property for refresh tokens
        public ICollection<RefreshToken>? RefreshTokens { get; set; }

        // Navigation property for the user's cart
        public Cart? Cart { get; set; }

        // Navigation property for orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        //public int UserId { get; set; }
        //public string? Username { get; set; }
        //public string? Lastname { get; set; }
        //[StringLength(100)]
        //public string? Firstname { get; set; }
        //public DateTime? Birthdate { get; set; }
        //public string PhoneNumber { get; set; } = string.Empty;
        ////public string Email { get; set; }
        //public ICollection<RefreshToken>? RefreshTokens { get; set; }
        //// Зв'язок з кошиком
        //public Cart Cart { get; set; }

        //// Зв'язок з замовленнями
        //public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
