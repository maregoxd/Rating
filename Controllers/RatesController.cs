using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rating.Models;

namespace Rating.Controllers
{
    public class RatesController : Controller
    {
        private readonly DBContext _context;

        public RatesController(DBContext context)
        {
            _context = context;
        }

        // GET: Rates
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.Rates.Include(r => r.Item).OrderByDescending(r => r.Value);
            return View(await dBContext.ToListAsync());
        }


        // GET: Rates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rate = await _context.Rates
                .Include(r => r.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rate == null)
            {
                return NotFound();
            }

            return View(rate);
        }

        // GET: Rates/Create
        public IActionResult Create(int id)
        {
            // Sprawdź, czy istnieje przedmiot o podanym id
            var item = _context.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            // Jeśli przedmiot istnieje, przekaż id do widoku
            ViewData["Item"] = id;
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id");
            return View();
        }

        // POST: Rates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Value,ItemId")] Rate rate)
        {
            // Sprawdź, czy model jest poprawny
            if (ModelState.IsValid)
            {
                // Sprawdź, czy istnieje przedmiot o podanym ItemId
                var item = _context.Items.Find(rate.ItemId);
                if (item == null)
                {
                    // Jeśli przedmiot nie istnieje, zwróć NotFound
                    return NotFound();
                }

                // Jeśli przedmiot istnieje, dodaj ocenę
                _context.Add(rate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Jeśli ModelState.IsValid jest false, zwróć widok z błędami walidacji
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", rate.ItemId);
            return View(rate);
        }


        // GET: Rates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rate = await _context.Rates.FindAsync(id);
            if (rate == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", rate.ItemId);
            return View(rate);
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Value,ItemId")] Rate rate)
        {
            if (id != rate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RateExists(rate.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", rate.ItemId);
            return View(rate);
        }

        // GET: Rates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rate = await _context.Rates
                .Include(r => r.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rate == null)
            {
                return NotFound();
            }

            return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rates == null)
            {
                return Problem("Entity set 'DBContext.Rates'  is null.");
            }
            var rate = await _context.Rates.FindAsync(id);
            if (rate != null)
            {
                _context.Rates.Remove(rate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RateExists(int id)
        {
          return _context.Rates.Any(e => e.Id == id);
        }
    }
}
