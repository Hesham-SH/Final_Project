using Core.Interfaces;
using Core.Models;
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

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("ProductOffers")]
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
        
        [HttpGet("/api/ProductReview/{id}")]
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
                        User = review.User.Firstname,
                        Description = review.Description
                    };
                    dTOs.Add(dto);
                }
                return Ok(dTOs);
            }
        }

        [HttpPost("{userId}/{productId}/{review}")]
        public async Task<ActionResult<ReviewsOnProduct>> ModifyReview(int userId, int productId, string review)
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

        [HttpGet("Product/Brand")]
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

    }
}
