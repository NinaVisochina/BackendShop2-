namespace BackendShop.Core.Dto.Category
{
    public class CategoryEditDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public string? Description { get; set; }
        public IFormFile? ImageCategory { get; set; }
    }
}
