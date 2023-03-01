namespace Core.Models
{
    public class ShoppingOrder : BaseModel
    {
        public string OrderName { get; set; }
        public string? Address { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<ShoppingOrderProduct> ShoppingOrderProducts { get; set; }
    }
}
