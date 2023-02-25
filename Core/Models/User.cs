using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models 
{
    public class User : BaseModel
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? Photo { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public List<Coupon> Coupons { get; set; }
        public List<ShoppingOrder> ShoppingOrders{ get; set; }

        public List<ReviewsOnProduct> ProductsReviewed { get; set; }
    }
}
