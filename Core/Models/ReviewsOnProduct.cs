namespace Core.Models
{
    public class ReviewsOnProduct : BaseModel
    {
        public string Description  { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
