using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Product : BaseModel
    {
        public string? Name { get; set; }
        [Column(TypeName ="money")]
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public string? Color { get; set; }
        public int? HasOffer { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public List<ReviewsOnProduct> UsersReviewed { get; set; }
        public List<ShoppingOrderProduct> ShoppingOrderProducts { get; set; }
    }
}
