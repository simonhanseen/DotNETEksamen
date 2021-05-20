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
    [Route("Categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CatalogContext _catalogContext;
        public CategoryController(ILogger<CategoryController> logger, CatalogContext catalogContext)
        {
            _logger = logger;
            _catalogContext = catalogContext;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _catalogContext.Categories.ToListAsync();

            return categories != null ? Ok(categories) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _catalogContext.Categories.
                FirstOrDefaultAsync(c => c.Id == id);

            return category != null ? Ok(category) : NotFound();
        }

        [HttpPost("")]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category newCategory)
        {

            await _catalogContext.Categories.AddAsync(newCategory);
            await _catalogContext.SaveChangesAsync();

            return newCategory != null ? Ok(newCategory) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id,[FromBody] Category newCategory)
        {
            newCategory.Id = id;
            _catalogContext.Set<Category>().Update(newCategory);
            await _catalogContext.SaveChangesAsync();

            return newCategory != null ? Ok(newCategory) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _catalogContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                _catalogContext.Categories.Remove(category);
                await _catalogContext.SaveChangesAsync();
            }

            return category != null ? Ok(category) : NotFound();
        }
    }
}
