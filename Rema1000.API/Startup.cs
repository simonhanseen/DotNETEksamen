using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Rema1000.Core;
using Rema1000.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rema1000.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rema1000.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                SeedDummyData(provider);

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rema1000.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SeedDummyData(IServiceProvider provider)
        {
            var catalogContext = provider.GetService<CatalogContext>();
            catalogContext?.AddRange(CreateDummyProducts());
            catalogContext?.SaveChanges();
        }

        private static IEnumerable<Product> CreateDummyProducts()
        {
            var rnd = new Random();
            string Unit = "";
            var result = new List<Product>();
            for (var i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                    Unit = "Unit";
                else
                    Unit = "Kg";

                var productId = Guid.NewGuid();
                result.Add(new Product()
                {
                    Id = productId,
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    Unit = Unit,
                    AmountInPackage = rnd.Next(1, 20),
                    Price = rnd.NextDouble() * 1000,
                    Discount = null,
                    Category = new Category() { Id = i, Name = $"Category {i}", Description = $"Description {i}" },
                    AmountInStock = rnd.Next(0, 1000),
                    Supplier = new Supplier() { Id = i, Name = $"Supplier {i}", Address = $"Mockstreet {i}", ZipCode = Convert.ToInt32($"{i}{i}{i}{i}"), Contact = "Mock Contact", Email = $"MockEmail{i}@Mockmail.com", PhoneNumber = $"+ 45 {i}{i} {i}{i} {i}{i} {i}{i}" }
                }) ;
            }

            return result;
        }
    }
}
