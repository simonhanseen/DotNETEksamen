using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rema1000.Core;
using Rema1000.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rema1000.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly CatalogContext _catalogContext;
        public ProductController(ILogger<ProductController> logger, CatalogContext catalogContext)
        {
            _logger = logger;
            _catalogContext = catalogContext;
        }

        [HttpGet("Products")]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var test = await _catalogContext.Products.ToListAsync();
            return test;

        }

        [HttpGet("Product")]
        public async Task<Product> GetProduct(Guid id)
        {
            return await _catalogContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        [HttpPost("Product")]
        public async Task<Product> CreateProduct([FromBody] Product newProduct)
        {
            await _catalogContext.Products.AddAsync(newProduct);
            await _catalogContext.SaveChangesAsync();
            return newProduct;
        }

        [HttpPut("Product")]
        public async Task<Product> UpdateProduct([FromBody] Product newProduct)
        {
            _catalogContext.Set<Product>().Update(newProduct);
            await _catalogContext.SaveChangesAsync();
            return newProduct;
        }

        [HttpDelete("Product")]
        public async Task DeleteProduct(Guid id)
        {
            var product = await _catalogContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            _catalogContext.Products.Remove(product);
            await _catalogContext.SaveChangesAsync();
        }
    }
}
