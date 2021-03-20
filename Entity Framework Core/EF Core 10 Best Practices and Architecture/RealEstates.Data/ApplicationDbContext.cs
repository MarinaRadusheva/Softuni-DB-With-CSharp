using Microsoft.EntityFrameworkCore;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Property> Properties { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server = .\SQLEXPRESS; Database = RealEstates; Integrated security = true;");
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
