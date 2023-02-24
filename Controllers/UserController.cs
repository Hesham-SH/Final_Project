using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SiteContext _context;
        public UserController(SiteContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingOrder>> GetOrders(int id)
        {
            var shoppingOrders = await _context.ShoppingOrderProducts.Include(S => S.Product).Include(S => S.ShoppingOrder).Where(S => S.ShoppingOrder.UserId == id).GroupBy(S => S.ShoppingOrder.OrderName).ToListAsync();
            List<ShoppingOrderProductDTO> orderProductDTOs = new List<ShoppingOrderProductDTO>();
            //List<string> productNames = new List<string>();
            foreach(var item in shoppingOrders)
            {
                
                ShoppingOrderProductDTO orderProductDTO = new ShoppingOrderProductDTO
                {
                    OrderName = item.Key,
                    Products = new List<string>()
                };
                foreach (var x in item)
                {
                    orderProductDTO.Products.Add(x.Product.Name);
                };
                orderProductDTOs.Add(orderProductDTO);
            }
            return Ok(orderProductDTOs);
        }
    }
}

            
