using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ShoppingOrderProductDTO
    {

        public string OrderName{ get; set; }
        public List<string> Products { get; set; }
    }
}
