using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class SiteContext : DbContext
    {
        public SiteContext(DbContextOptions opt) : base(opt)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<ShoppingOrder> ShoppingOrders { get; set; }
        public DbSet<ShoppingOrderProduct> ShoppingOrderProducts { get; set; }
        public DbSet<ReviewsOnProduct> ReviewsOnProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ReviewsOnProduct>().HasKey(R => new { R.UserId, R.ProductId });
            modelBuilder.Entity<ShoppingOrderProduct>().HasKey(S => new { S.ShoppingOrderid, S.ProductId});
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        }
    }
