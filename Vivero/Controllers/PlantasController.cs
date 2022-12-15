using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vivero.Context;
using Vivero.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Vivero.Controllers
{
    public class PlantasController : Controller
    {
        private readonly ViveroContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PlantasController(ViveroContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

        // GET: Plantas
        public async Task<IActionResult> Index()
        {
            var viveroContext = _context.Plantas.Include(p => p.JardinerosNavigation);
            return View(await viveroContext.ToListAsync());
        }

        // GET: Plantas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Plantas == null)
            {
                return NotFound();
            }

            var plantas = await _context.Plantas
                .Include(p => p.JardinerosNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plantas == null)
            {
                return NotFound();
            }

            return View(plantas);
        }

        // GET: Plantas/Create
        public IActionResult Create()
        {
            ViewData["Jardineros"] = new SelectList(_context.Jardineros, "Id", "Codigo");
            return View();
        }

        // POST: Plantas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Plantas plantas)
        {
            if (ModelState.IsValid)
            {
                string uFilename = UploadedFile(plantas);
                plantas.Imagen = uFilename;
                _context.Add(plantas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Jardineros"] = new SelectList(_context.Jardineros, "Id", "Codigo", plantas.Jardineros);
            return View(plantas);
        }

        // GET: Plantas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Plantas == null)
            {
                return NotFound();
            }

            var plantas = await _context.Plantas.FindAsync(id);
            if (plantas == null)
            {
                return NotFound();
            }
            ViewData["Jardineros"] = new SelectList(_context.Jardineros, "Id", "Codigo", plantas.Jardineros);
            return View(plantas);
        }

        // POST: Plantas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Valor,Imagen,Jardineros")] Plantas plantas)
        {
            if (id != plantas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantasExists(plantas.Id))
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
            ViewData["Jardineros"] = new SelectList(_context.Jardineros, "Id", "Codigo", plantas.Jardineros);
            return View(plantas);
        }

        // GET: Plantas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Plantas == null)
            {
                return NotFound();
            }

            var plantas = await _context.Plantas
                .Include(p => p.JardinerosNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plantas == null)
            {
                return NotFound();
            }

            return View(plantas);
        }

        // POST: Plantas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Plantas == null)
            {
                return Problem("Entity set 'ViveroContext.Plantas'  is null.");
            }
            var plantas = await _context.Plantas.FindAsync(id);
            if (plantas != null)
            {
                _context.Plantas.Remove(plantas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantasExists(int id)
        {
          return (_context.Plantas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private string UploadedFile(Plantas plantas)
        {
            string uFileNme = null;
            if (plantas.ImagenFile != null)
            {
                string uploadsdFolder = Path.Combine(webHostEnvironment.WebRootPath, "Img");
                uFileNme = Guid.NewGuid().ToString() + "_" + plantas.ImagenFile.FileName;
                string filePath = Path.Combine(uploadsdFolder, uFileNme);
                using (var myFileStream = new FileStream(filePath, FileMode.Create))
                {
                    plantas.ImagenFile.CopyTo(myFileStream);
                }
            }
            return uFileNme;
        }
    }
}
