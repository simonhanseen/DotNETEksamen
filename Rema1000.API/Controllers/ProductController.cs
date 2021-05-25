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
    [Route("[controller]s")]
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

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _catalogContext.Products.
                Include(x => x.Unit).
                Include(x => x.Supplier).
                Include(x => x.Category).
                ToListAsync();

            return products != null ? Ok(products) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _catalogContext.Products.
                Include(x => x.Unit).
                Include(x => x.Supplier).
                Include(x => x.Category).
                FirstOrDefaultAsync(p => p.Id == id);

            return product != null ? Ok(product) : NotFound();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Product>> CreateProduct(Guid id, [FromBody] Product newProduct)
        {
            newProduct.Id = id;

            await _catalogContext.Products.AddAsync(newProduct);
            await _catalogContext.SaveChangesAsync();

            return newProduct != null ? Ok(newProduct) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(Guid id, [FromBody] Product newProduct)
        {

            newProduct.Id = id;

            _catalogContext.Set<Product>().Update(newProduct);

            try
            {
                await _catalogContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return newProduct != null ? Ok(newProduct) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(Guid id)
        {
            var product = await _catalogContext.Products.
                Include(x => x.Unit).
                Include(x => x.Supplier).
                Include(x => x.Category).
                FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                _catalogContext.Products.Remove(product);
                await _catalogContext.SaveChangesAsync();
            }

            return product != null ? Ok(product) : NotFound();
        }
    }
}
