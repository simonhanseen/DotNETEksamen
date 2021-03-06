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
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly CatalogContext _catalogContext;
        public SupplierController(ILogger<SupplierController> logger, CatalogContext catalogContext)
        {
            _logger = logger;
            _catalogContext = catalogContext;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            var categories = await _catalogContext.Suppliers.ToListAsync();

            return categories != null ? Ok(categories) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _catalogContext.Suppliers.
                FirstOrDefaultAsync(s => s.Id == id);

            return supplier != null ? Ok(supplier) : NotFound();
        }
        /// <summary>
        /// Id is required here, but normally the database would set the id on its own
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newSupplier"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<Supplier>> CreateSupplier(int id, [FromBody] Supplier newSupplier)
        {
            newSupplier.Id = id;

            var exists = _catalogContext.Suppliers.Any(x => x.Id == id);

            if (exists)
                return BadRequest("Id already exists");

            await _catalogContext.Suppliers.AddAsync(newSupplier);
            await _catalogContext.SaveChangesAsync();

            return newSupplier != null ? Ok(newSupplier) : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Supplier>> UpdateSupplier(int id, [FromBody] Supplier newSupplier)
        {
            newSupplier.Id = id;

            _catalogContext.Set<Supplier>().Update(newSupplier);
            try
            {
                await _catalogContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return newSupplier != null ? Ok(newSupplier) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Supplier>> DeleteSupplier(int id)
        {
            var supplier = await _catalogContext.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

            if (supplier != null)
            {
                _catalogContext.Suppliers.Remove(supplier);
                await _catalogContext.SaveChangesAsync();
            }

            return supplier != null ? Ok(supplier) : NotFound();
        }
    }
}
