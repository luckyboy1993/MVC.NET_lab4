using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DALlab3.Entities;

namespace lab3.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Stores")]
    public class StoresController : Controller
    {
        private readonly AdventureWorks2014Context _context;

        public StoresController(AdventureWorks2014Context context)
        {
            _context = context;
        } 

        // GET: api/Stores
        [HttpGet]
        public IEnumerable<Store> GetStore([FromQuery] string query)
        {
            if (query != null)
                return _context.Store.Where(p => p.Name.Contains(query));

            return _context.Store;
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = await _context.Store.Include(s => s.Customer).FirstOrDefaultAsync(m => m.BusinessEntityId == id);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        // PUT: api/Stores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore([FromRoute] int id, [FromBody] Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != store.BusinessEntityId)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Stores
        [HttpPost]
        public async Task<IActionResult> PostStore([FromBody] Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Store.Add(store);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreExists(store.BusinessEntityId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStore", new { id = store.BusinessEntityId }, store);
        }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = await _context.Store.SingleOrDefaultAsync(m => m.BusinessEntityId == id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Store.Remove(store);
            await _context.SaveChangesAsync();

            return Ok(store);
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.BusinessEntityId == id);
        }
    }
}