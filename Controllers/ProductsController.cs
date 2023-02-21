using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public string GetProducts() =>
            "A List Of Products";

        [HttpGet("{id}")]
        public string GetProduct(int id) =>
            "A List Of Products";
    }
}
