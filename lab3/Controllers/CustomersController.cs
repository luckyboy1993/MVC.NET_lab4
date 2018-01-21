using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DALlab3.Entities;

namespace lab3.Controllers
{
    public class CustomersController : Controller
    {
        private readonly _Context _context;

        public CustomersController(_Context context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var adventureWorks2014Context = _context.Customer.Include(c => c.Person).Include(c => c.Store).Include(c => c.Territory).Take(100);
            return View(await adventureWorks2014Context.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.Person)
                .Include(c => c.Store)
                .Include(c => c.Territory)
                .SingleOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Person, "BusinessEntityId", "FirstName");
            ViewData["StoreId"] = new SelectList(_context.Store, "BusinessEntityId", "Name");
            ViewData["TerritoryId"] = new SelectList(_context.SalesTerritory, "TerritoryId", "CountryRegionCode");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,PersonId,StoreId,TerritoryId,AccountNumber,Rowguid,ModifiedDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Person, "BusinessEntityId", "FirstName", customer.PersonId);
            ViewData["StoreId"] = new SelectList(_context.Store, "BusinessEntityId", "Name", customer.StoreId);
            ViewData["TerritoryId"] = new SelectList(_context.SalesTerritory, "TerritoryId", "CountryRegionCode", customer.TerritoryId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["PersonId"] = new SelectList(_context.Person, "BusinessEntityId", "FirstName", customer.PersonId);
            ViewData["StoreId"] = new SelectList(_context.Store, "BusinessEntityId", "Name", customer.StoreId);
            ViewData["TerritoryId"] = new SelectList(_context.SalesTerritory, "TerritoryId", "CountryRegionCode", customer.TerritoryId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,PersonId,StoreId,TerritoryId,AccountNumber,Rowguid,ModifiedDate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PersonId"] = new SelectList(_context.Person, "BusinessEntityId", "FirstName", customer.PersonId);
            ViewData["StoreId"] = new SelectList(_context.Store, "BusinessEntityId", "Name", customer.StoreId);
            ViewData["TerritoryId"] = new SelectList(_context.SalesTerritory, "TerritoryId", "CountryRegionCode", customer.TerritoryId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.Person)
                .Include(c => c.Store)
                .Include(c => c.Territory)
                .SingleOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.CustomerId == id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
