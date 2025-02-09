namespace BackendShop.Core.Dto
{
    public class UserTokens
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string? UserId { get; set; } // Нове поле
        public bool IsAdmin { get; set; } // Додаємо поле для ролі

    }
}
