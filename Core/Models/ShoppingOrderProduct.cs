namespace Core.Models
{
    public class ShoppingOrderProduct : BaseModel
    {
        public int ProductId { get; set; }
        public Product Product{ get; set; }
        public int ShoppingOrderid { get; set; }
        public ShoppingOrder ShoppingOrder{ get; set; }
    }
}
