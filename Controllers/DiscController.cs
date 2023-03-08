using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDiscs.Data;
using MyDiscs.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MyDiscs.Controllers
{

    //only loged in users can access this controller
    [Authorize]
    public class DiscController : Controller
    {
        private readonly MydiscContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private string wwwRootPath;


        public DiscController(MydiscContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = _hostEnvironment.WebRootPath;
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
        public async Task<IActionResult> Create([Bind("DiscId,Name,Speed,Glide,Turn,Fade,Plastic,Bagged,ImageFile,CategoryId,BrandId")] Disc disc)
        {
            if (ModelState.IsValid)
            {

                //check for image file 
                if (disc.ImageFile != null)
                {
                    //save img to wwwroot/uploads 
                    //separate the filename and extension
                    string filename = Path.GetFileNameWithoutExtension(disc.ImageFile.FileName);
                    string extension = Path.GetExtension(disc.ImageFile.FileName);

                    //create new filename with timestamp 
                    disc.ImageName = filename = filename.Replace(" ", string.Empty) + DateTime.Now.ToString("yymmssfff") + extension;
                    //create the complete file path 
                    string path = Path.Combine(wwwRootPath + "/uploads/", filename);

                    //save file 
                    using (var fileStream = new FileStream(path, FileMode.Create)){
                        await disc.ImageFile.CopyToAsync(fileStream);
                    }

                    //create thumbnails and webp 
                    ImageFile(filename);
                }else{
                    disc.ImageName = "standard.png";
                }

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

        //Search method 
        public async Task<IActionResult> Search(string searchString)
        {
            //return index view if string is null or empty 
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return View("Index", await _context.Discs.ToListAsync());
            }

            //if string is not null or empty, return result to view. 
            //check disc namn, brand namne and category name for matches
            var discs = await _context.Discs.Include(d => d.Brand)
                .Include(d => d.Category)
                .Where(d => d.Name.ToLower().Contains(searchString.ToLower()) ||
                            d.Brand.BrandName.ToLower().Contains(searchString.ToLower()) ||
                            d.Category.CategoryName.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();

            //if no matches, return message 
            if (discs.Count == 0)
            {
                ViewBag.Message = searchString + " not found";
            }
            return View("Index", discs);
        }

        //image file method 
        public void ImageFile(string filename)
        {
            //path 
            string path = wwwRootPath + "/uploads/";

            using var img = Image.Load(path + filename);
            img.Mutate(x => x.Resize(250, 250));
            img.Save(path + "thumb_" + filename);

            //WebP version 
            using var imgWebp = Image.Load(path + filename);
            string webp = filename.Substring(0, filename.LastIndexOf(".", StringComparison.Ordinal)) + ".webp";
            imgWebp.SaveAsWebp(path + webp);
        }

    }
}
