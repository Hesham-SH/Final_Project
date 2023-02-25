using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<Product>> GetProductsHasOffersAsync();
        Task<IReadOnlyList<ReviewsOnProduct>> GetAllReviewsAsync(int id);
        Task<ReviewsOnProduct> AddOrUpdateReviewAsync(int userId, int productId, string review);
        Task<IReadOnlyList<Product>> GetProductByCategoryAsync(int id);
        Task<IReadOnlyList<Product>> GetProductByBrandAsync(int id);
    }
}
