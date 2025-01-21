namespace BackendShop.Core.Dto.Category
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        //public string? Description { get; set; }
        public IFormFile? ImageCategory { get; set; }
    }
}
