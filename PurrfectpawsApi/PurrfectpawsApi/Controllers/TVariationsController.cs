using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurrfectpawsApi.DatabaseDbContext;
using PurrfectpawsApi.Models;

namespace PurrfectpawsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TVariationsController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TVariationsController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TVariations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TVariation>>> GetTVariations()
        {
          if (_context.TVariations == null)
          {
              return NotFound();
          }
            return await _context.TVariations.ToListAsync();
        }

        // GET: api/TVariations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TVariation>> GetTVariation(int id)
        {
          if (_context.TVariations == null)
          {
              return NotFound();
          }
            var tVariation = await _context.TVariations.FindAsync(id);

            if (tVariation == null)
            {
                return NotFound();
            }

            return tVariation;
        }

        // PUT: api/TVariations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTVariation(int id, TVariation tVariation)
        {
            if (id != tVariation.VariationId)
            {
                return BadRequest();
            }

            _context.Entry(tVariation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TVariationExists(id))
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

        // POST: api/TVariations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TVariation>> PostTVariation(TVariation tVariation)
        {
          if (_context.TVariations == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.TVariations'  is null.");
          }
            _context.TVariations.Add(tVariation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTVariation", new { id = tVariation.VariationId }, tVariation);
        }

        // DELETE: api/TVariations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTVariation(int id)
        {
            if (_context.TVariations == null)
            {
                return NotFound();
            }
            var tVariation = await _context.TVariations.FindAsync(id);
            if (tVariation == null)
            {
                return NotFound();
            }

            _context.TVariations.Remove(tVariation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TVariationExists(int id)
        {
            return (_context.TVariations?.Any(e => e.VariationId == id)).GetValueOrDefault();
        }
    }
}
