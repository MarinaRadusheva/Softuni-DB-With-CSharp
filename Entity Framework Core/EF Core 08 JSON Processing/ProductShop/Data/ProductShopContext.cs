using Microsoft.EntityFrameworkCore;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext()
        {

        }
        public ProductShopContext(DbContextOptions options):base(options)
        {

        }
       public DbSet<User> Users { get; set; }
       public DbSet<Product> Products { get; set; }
       public DbSet<Category> Categories { get; set; }
       public DbSet<CategoryProduct> CategoryProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
             base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>()
                .HasKey(x => new { x.CategoryId, x.ProductId });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
