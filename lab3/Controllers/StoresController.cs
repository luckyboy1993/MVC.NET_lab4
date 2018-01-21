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
    public class StoresController : Controller
    {
        private readonly _Context _context;

        public StoresController(_Context context)
        {
            _context = context;
        }

        public IActionResult AjaxStores()
        {
            return View();
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            var adventureWorks2014Context = _context.Store.Include(s => s.BusinessEntity).Include(s => s.SalesPerson);
            return View(await adventureWorks2014Context.ToListAsync());
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(s => s.BusinessEntity)
                .Include(s => s.SalesPerson)
                .SingleOrDefaultAsync(m => m.BusinessEntityId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntity, "BusinessEntityId", "BusinessEntityId");
            ViewData["SalesPersonId"] = new SelectList(_context.SalesPerson, "BusinessEntityId", "BusinessEntityId");
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusinessEntityId,Name,SalesPersonId,Demographics,Rowguid,ModifiedDate")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntity, "BusinessEntityId", "BusinessEntityId", store.BusinessEntityId);
            ViewData["SalesPersonId"] = new SelectList(_context.SalesPerson, "BusinessEntityId", "BusinessEntityId", store.SalesPersonId);
            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.SingleOrDefaultAsync(m => m.BusinessEntityId == id);
            if (store == null)
            {
                return NotFound();
            }
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntity, "BusinessEntityId", "BusinessEntityId", store.BusinessEntityId);
            ViewData["SalesPersonId"] = new SelectList(_context.SalesPerson, "BusinessEntityId", "BusinessEntityId", store.SalesPersonId);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusinessEntityId,Name,SalesPersonId,Demographics,Rowguid,ModifiedDate")] Store store)
        {
            if (id != store.BusinessEntityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.BusinessEntityId))
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
            ViewData["BusinessEntityId"] = new SelectList(_context.BusinessEntity, "BusinessEntityId", "BusinessEntityId", store.BusinessEntityId);
            ViewData["SalesPersonId"] = new SelectList(_context.SalesPerson, "BusinessEntityId", "BusinessEntityId", store.SalesPersonId);
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(s => s.BusinessEntity)
                .Include(s => s.SalesPerson)
                .SingleOrDefaultAsync(m => m.BusinessEntityId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Store.SingleOrDefaultAsync(m => m.BusinessEntityId == id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.BusinessEntityId == id);
        }
    }
}
