using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly SiteContext _context;
        public ProductRepository(SiteContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id) =>
            await _context.Products.FindAsync(id);

        public async Task<IReadOnlyList<Product>> GetProductsAsync() =>
            await _context.Products.ToListAsync();

        public async Task<IReadOnlyList<Product>> GetProductsHasOffersAsync() =>
            await _context.Products.Select(P => P).Where(P => P.HasOffer != null).ToListAsync();

        public async Task<IReadOnlyList<ReviewsOnProduct>> GetAllReviewsAsync(int id) =>
            await _context.ReviewsOnProducts.Where(R => R.ProductId == id).Include(R => R.Product).Include(R => R.User).ToListAsync();

        public async Task<ReviewsOnProduct> AddOrUpdateReviewAsync(int userId, int productId, string review) =>
            await _context.ReviewsOnProducts.Where(R => R.UserId == userId && R.ProductId == productId).SingleOrDefaultAsync();

        public async Task<IReadOnlyList<Product>> GetProductByCategoryAsync(int id) =>
            await _context.Products.Where(P => P.CategoryId == id).ToListAsync();

        public async Task<IReadOnlyList<Product>> GetProductByBrandAsync(int id) =>
            await _context.Products.Where(P => P.BrandId == id).ToListAsync();
    }
}
