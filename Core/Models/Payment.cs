namespace Core.Models
{
    public class Payment : BaseModel
    {
        public string Method { get; set; }
        public List<ShoppingOrder> ShoppingOrders { get; set; }
    }
}
