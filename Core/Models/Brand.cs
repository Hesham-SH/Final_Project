using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Brand : BaseModel
    {
        public string? Name { get; set; }
        public int? Image { get; set; }
        public List<Product> Products { get; set; }
        
    }
}
