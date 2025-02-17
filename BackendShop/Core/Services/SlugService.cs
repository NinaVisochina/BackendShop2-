namespace BackendShop.Core.Services
{
    public static class SlugService
    {
        public static string GenerateSlug(string name)
        {
            return name.ToLower()
                       .Trim()
                       .Replace(" ", "-")
                       .Replace("'", "")
                       .Replace("\"", "")
                       .Replace("?", "")
                       .Replace("!", "")
                       .Replace(",", "")
                       .Replace(".", "")
                       .Replace("&", "and");
        }
    }

}
