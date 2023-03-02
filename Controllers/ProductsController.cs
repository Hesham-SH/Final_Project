using Core.Interfaces;
using Core.Models;
using Core.Models.DTOS;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Final_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly SiteContext _context;
        public ProductsController(IProductRepository repository, SiteContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("GetProductWithOffers")]
        public async Task<ActionResult<List<Product>>> GetProductsWithOffers()
        {
            var products = await _repository.GetProductsHasOffersAsync();
            if (products.Count != 0)
            {
                return Ok(products);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            return Ok(product);
        }
        
        [HttpGet("GetReviewOnProduct/{id}")]
        public async Task<ActionResult<List<ReviewsOnProduct>>> GetReview(int id)
        {
            var result = await _repository.GetAllReviewsAsync(id);
            if (result.Count == 0)
            {
                return NotFound();
            }
            else
            {
                List<ReviewOnProductDTO> dTOs = new List<ReviewOnProductDTO>();
                foreach (ReviewsOnProduct review in result)
                {
                    ReviewOnProductDTO dto = new ReviewOnProductDTO()
                    {
                        ProductName = review.Product.Name,
                        User = review.User.UserName,
                        Description = review.Description
                    };
                    dTOs.Add(dto);
                }
                return Ok(dTOs);
            }
        }

        [HttpPost("AddOrUpdateReviewOnProduct/{userId}/{productId}/{review}")]
        public async Task<ActionResult<ReviewsOnProduct>> ModifyReview(string userId, int productId, string review)
        {
            var result = await _repository.AddOrUpdateReviewAsync(userId, productId, review);
            if (result != null)
            {
                result.Description = review;
                _context.SaveChanges();
                return Ok("Review Updated");
            }
            else
            {
                ReviewsOnProduct reviewsOnProduct = new ReviewsOnProduct()
                {
                    UserId = userId,
                    ProductId = productId,
                    Description = review

                };
                _context.ReviewsOnProducts.Add(reviewsOnProduct);
                _context.SaveChanges();
                return Ok("Review Made");
            }
        }

        [HttpGet("GetProductByBrand")]
        public async Task<ActionResult<List<Product>>> GetProductsByBrand(int id)
        {
            var result = await _repository.GetProductByBrandAsync(id);
            if(result.Count != 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/Product/Category")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(int id)
        {
            var result = await _repository.GetProductByCategoryAsync(id);
            if (result.Count != 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddShoppingOrder(InsertShoppingOrderDTO insertShoppingOrderDTO)
        {
            //add into shopping order
            ShoppingOrder shoppingOrder = new ShoppingOrder()
            {
                OrderName = insertShoppingOrderDTO.OrderName,
                UserId = insertShoppingOrderDTO.UserId,
                PaymentId = insertShoppingOrderDTO.PaymentId
            };

            var result = await _context.ShoppingOrders.AddAsync(shoppingOrder);
            _context.SaveChanges();
            var query = await _context.ShoppingOrders.Where(SO => SO.OrderName == insertShoppingOrderDTO.OrderName).SingleOrDefaultAsync();
            ShoppingOrdersDTO shoppingOrdersDTO = new ShoppingOrdersDTO()
            {
                OrderId = query.Id,
                OrderName = query.OrderName,
                PaymentId = query.PaymentId,
                UserId = query.UserId
            };
            return Ok(shoppingOrdersDTO);
        }
        [HttpPost("AddProductsToShoppingOrder")]
        public async Task<ActionResult> AddProductsToShoppingOrder(InsertShoppingOrderProductDTO insertShoppingOrderProductDTO)
        {
            ShoppingOrder shoppingOrder = new ShoppingOrder()
            {
                OrderName = insertShoppingOrderProductDTO.OrderName,
                UserId = insertShoppingOrderProductDTO.UserId,
                PaymentId = insertShoppingOrderProductDTO.PaymentId
            };

            var result = await _context.ShoppingOrders.AddAsync(shoppingOrder);
            _context.SaveChanges();
            var query = await _context.ShoppingOrders.Where(SO => SO.OrderName == insertShoppingOrderProductDTO.OrderName).SingleOrDefaultAsync();
            ShoppingOrdersDTO shoppingOrdersDTO = new ShoppingOrdersDTO()
            {
                OrderId = query.Id,
                OrderName = query.OrderName,
                PaymentId = query.PaymentId,
                UserId = query.UserId
            };
            foreach(var item in insertShoppingOrderProductDTO.Products)
            {
                ShoppingOrderProduct shoppingOrderProduct = new ShoppingOrderProduct()
                {
                    ShoppingOrderid = shoppingOrdersDTO.OrderId,
                    ProductId = item.ProductId
                };
                await _context.ShoppingOrderProducts.AddAsync(shoppingOrderProduct);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("GetTopSellingProdcut")]
        public async Task<ActionResult<List<ProductDTO>>> GetTopSellingProdcut()
        {
            var result = await _repository.GetBestSellerAsync();
            if(result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }






        [HttpGet("GetUserReviews{id}")]
        public async Task<ActionResult<ShoppingOrder>> GetOrders(string id)
        {
            var shoppingOrders = await _context.ShoppingOrderProducts.Include(S => S.Product).Include(S => S.ShoppingOrder).Where(S => S.ShoppingOrder.UserId == id).GroupBy(S => S.ShoppingOrder.OrderName).ToListAsync();
            List<ShoppingOrderProductDTO> orderProductDTOs = new List<ShoppingOrderProductDTO>();
            foreach (var item in shoppingOrders)
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
