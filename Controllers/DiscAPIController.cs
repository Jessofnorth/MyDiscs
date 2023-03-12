using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDiscs.Data;
using MyDiscs.Models;

namespace MyDiscs.Controllers
{
    // API controller for Disc model with Crud functionality

    [Route("api/disc")]
    [ApiController]
    public class DiscAPIController : ControllerBase
    {
        private readonly MydiscContext _context;

        public DiscAPIController(MydiscContext context)
        {
            _context = context;
        }

        // GET: api/Disc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disc>>> GetDiscs()
        {
            if (_context.Discs == null)
            {
                return NotFound();
            }
            //include brand and category info to result
            return await _context.Discs.Include(d => d.Brand).Include(d => d.Category).ToListAsync();
        }

        // GET: api/Disc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Disc>> GetDisc(int id)
        {
            if (_context.Discs == null)
            {
                return NotFound();
            }
            //include brand and category info to result
            var disc = await _context.Discs.Include(d => d.Brand).Include(d => d.Category).FirstOrDefaultAsync(m => m.DiscId == id);

            if (disc == null)
            {
                return NotFound();
            }

            return disc;
        }

        // PUT: api/Disc/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisc(int id, Disc disc)
        {
            if (id != disc.DiscId)
            {
                return BadRequest();
            }

            _context.Entry(disc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _context.Discs.Include(d => d.Brand).Include(d => d.Category).FirstOrDefaultAsync(m => m.DiscId == id);
            return CreatedAtAction("GetDisc", new { id = disc.DiscId }, disc);
        }

        // POST: api/Disc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Disc>> PostDisc(Disc disc)
        {
            if (_context.Discs == null)
            {
                return Problem("Entity set 'MydiscContext.Discs'  is null.");
            }
            _context.Discs.Add(disc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisc", new { id = disc.DiscId }, disc);
        }

        // DELETE: api/Disc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisc(int id)
        {
            if (_context.Discs == null)
            {
                return NotFound();
            }
            var disc = await _context.Discs.FindAsync(id);
            if (disc == null)
            {
                return NotFound();
            }

            _context.Discs.Remove(disc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiscExists(int id)
        {
            return (_context.Discs?.Any(e => e.DiscId == id)).GetValueOrDefault();
        }
    }
}
