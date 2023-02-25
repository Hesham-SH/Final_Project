namespace Core.Models
{
    public class Coupon : BaseModel
    {
        public string? Code { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
