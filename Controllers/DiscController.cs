using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDiscs.Data;
using MyDiscs.Models;

namespace MyDiscs.Controllers
{
    public class DiscController : Controller
    {
        private readonly MydiscContext _context;

        public DiscController(MydiscContext context)
        {
            _context = context;
        }

        // GET: Disc
        public async Task<IActionResult> Index()
        {
            var mydiscContext = _context.Discs.Include(d => d.Brand).Include(d => d.Category);
            return View(await mydiscContext.ToListAsync());
        }

        // GET: Disc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Discs == null)
            {
                return NotFound();
            }

            var disc = await _context.Discs
                .Include(d => d.Brand)
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DiscId == id);
            if (disc == null)
            {
                return NotFound();
            }

            return View(disc);
        }

        // GET: Disc/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Disc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscId,Name,Speed,Glide,Turn,Fade,Plastic,Bagged,ImageName,CategoryId,BrandId")] Disc disc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", disc.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", disc.CategoryId);
            return View(disc);
        }

        // GET: Disc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Discs == null)
            {
                return NotFound();
            }

            var disc = await _context.Discs.FindAsync(id);
            if (disc == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", disc.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", disc.CategoryId);
            return View(disc);
        }

        // POST: Disc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscId,Name,Speed,Glide,Turn,Fade,Plastic,Bagged,ImageName,CategoryId,BrandId")] Disc disc)
        {
            if (id != disc.DiscId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscExists(disc.DiscId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", disc.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", disc.CategoryId);
            return View(disc);
        }

        // GET: Disc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Discs == null)
            {
                return NotFound();
            }

            var disc = await _context.Discs
                .Include(d => d.Brand)
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DiscId == id);
            if (disc == null)
            {
                return NotFound();
            }

            return View(disc);
        }

        // POST: Disc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Discs == null)
            {
                return Problem("Entity set 'MydiscContext.Discs'  is null.");
            }
            var disc = await _context.Discs.FindAsync(id);
            if (disc != null)
            {
                _context.Discs.Remove(disc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscExists(int id)
        {
          return (_context.Discs?.Any(e => e.DiscId == id)).GetValueOrDefault();
        }
    }
}
