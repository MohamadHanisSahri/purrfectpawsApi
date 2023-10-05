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
    public class TLeadLengthsController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TLeadLengthsController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TLeadLengths
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TLeadLength>>> GetTLeadLengths()
        {
          if (_context.TLeadLengths == null)
          {
              return NotFound();
          }
            return await _context.TLeadLengths.ToListAsync();
        }

        // GET: api/TLeadLengths/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TLeadLength>> GetTLeadLength(int id)
        {
          if (_context.TLeadLengths == null)
          {
              return NotFound();
          }
            var tLeadLength = await _context.TLeadLengths.FindAsync(id);

            if (tLeadLength == null)
            {
                return NotFound();
            }

            return tLeadLength;
        }

        // PUT: api/TLeadLengths/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTLeadLength(int id, TLeadLength tLeadLength)
        {
            if (id != tLeadLength.LeadLengthId)
            {
                return BadRequest();
            }

            _context.Entry(tLeadLength).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TLeadLengthExists(id))
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

        // POST: api/TLeadLengths
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TLeadLength>> PostTLeadLength(TLeadLength tLeadLength)
        {
          if (_context.TLeadLengths == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.TLeadLengths'  is null.");
          }
            _context.TLeadLengths.Add(tLeadLength);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTLeadLength", new { id = tLeadLength.LeadLengthId }, tLeadLength);
        }

        // DELETE: api/TLeadLengths/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTLeadLength(int id)
        {
            if (_context.TLeadLengths == null)
            {
                return NotFound();
            }
            var tLeadLength = await _context.TLeadLengths.FindAsync(id);
            if (tLeadLength == null)
            {
                return NotFound();
            }

            _context.TLeadLengths.Remove(tLeadLength);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TLeadLengthExists(int id)
        {
            return (_context.TLeadLengths?.Any(e => e.LeadLengthId == id)).GetValueOrDefault();
        }
    }
}
