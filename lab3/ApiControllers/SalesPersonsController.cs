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
    [Route("api/SalesPersons")]
    public class SalesPersonsController : Controller
    {
        private readonly _Context _context;

        public SalesPersonsController(_Context context)
        {
            _context = context;
        }

        // GET: api/SalesPersons
        [HttpGet]
        public IEnumerable<SalesPerson> GetSalesPerson()
        {
            return _context.SalesPerson;
        }

        // GET: api/SalesPersons/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesPerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesPerson = await _context.SalesPerson.SingleOrDefaultAsync(m => m.BusinessEntityId == id);

            if (salesPerson == null)
            {
                return NotFound();
            }

            return Ok(salesPerson);
        }

        // PUT: api/SalesPersons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesPerson([FromRoute] int id, [FromBody] SalesPerson salesPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salesPerson.BusinessEntityId)
            {
                return BadRequest();
            }

            _context.Entry(salesPerson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesPersonExists(id))
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

        // POST: api/SalesPersons
        [HttpPost]
        public async Task<IActionResult> PostSalesPerson([FromBody] SalesPerson salesPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SalesPerson.Add(salesPerson);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesPersonExists(salesPerson.BusinessEntityId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSalesPerson", new { id = salesPerson.BusinessEntityId }, salesPerson);
        }

        // DELETE: api/SalesPersons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesPerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesPerson = await _context.SalesPerson.SingleOrDefaultAsync(m => m.BusinessEntityId == id);
            if (salesPerson == null)
            {
                return NotFound();
            }

            _context.SalesPerson.Remove(salesPerson);
            await _context.SaveChangesAsync();

            return Ok(salesPerson);
        }

        private bool SalesPersonExists(int id)
        {
            return _context.SalesPerson.Any(e => e.BusinessEntityId == id);
        }
    }
}