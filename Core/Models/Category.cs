namespace Core.Models
{
    public class Category : BaseModel
    {
        public string? Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
