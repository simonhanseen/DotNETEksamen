using Microsoft.EntityFrameworkCore;
using Rema1000.Core;
using System;

namespace Rema1000.Infrastructure
{
    public class CatalogContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("Products");//options.UseSqlite("Data Source=products.db");
    }
}
