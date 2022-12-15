using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vivero.Context;
using Vivero.Models;

namespace Vivero.Controllers
{
    public class JardinerosController : Controller
    {
        private readonly ViveroContext _context;

        public JardinerosController(ViveroContext context)
        {
            _context = context;
        }

        // GET: Jardineros
        public async Task<IActionResult> Index()
        {
              return _context.Jardineros != null ? 
                          View(await _context.Jardineros.ToListAsync()) :
                          Problem("Entity set 'ViveroContext.Jardineros'  is null.");
        }

        // GET: Jardineros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jardineros == null)
            {
                return NotFound();
            }

            var jardineros = await _context.Jardineros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jardineros == null)
            {
                return NotFound();
            }

            return View(jardineros);
        }

        // GET: Jardineros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jardineros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Codigo")] Jardineros jardineros)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jardineros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jardineros);
        }

        // GET: Jardineros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jardineros == null)
            {
                return NotFound();
            }

            var jardineros = await _context.Jardineros.FindAsync(id);
            if (jardineros == null)
            {
                return NotFound();
            }
            return View(jardineros);
        }

        // POST: Jardineros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Codigo")] Jardineros jardineros)
        {
            if (id != jardineros.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jardineros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JardinerosExists(jardineros.Id))
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
            return View(jardineros);
        }

        // GET: Jardineros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jardineros == null)
            {
                return NotFound();
            }

            var jardineros = await _context.Jardineros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jardineros == null)
            {
                return NotFound();
            }

            return View(jardineros);
        }

        // POST: Jardineros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jardineros == null)
            {
                return Problem("Entity set 'ViveroContext.Jardineros'  is null.");
            }
            var jardineros = await _context.Jardineros.FindAsync(id);
            if (jardineros != null)
            {
                _context.Jardineros.Remove(jardineros);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JardinerosExists(int id)
        {
          return (_context.Jardineros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
