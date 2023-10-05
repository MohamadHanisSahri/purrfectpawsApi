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
    public class MSizesController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public MSizesController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/MSizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MSize>>> GetMSizes()
        {
          if (_context.MSizes == null)
          {
              return NotFound();
          }
            return await _context.MSizes.ToListAsync();
        }

        // GET: api/MSizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MSize>> GetMSize(int id)
        {
          if (_context.MSizes == null)
          {
              return NotFound();
          }
            var mSize = await _context.MSizes.FindAsync(id);

            if (mSize == null)
            {
                return NotFound();
            }

            return mSize;
        }

        // PUT: api/MSizes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMSize(int id, MSize mSize)
        {
            if (id != mSize.SizeId)
            {
                return BadRequest();
            }

            _context.Entry(mSize).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MSizeExists(id))
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

        // POST: api/MSizes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MSize>> PostMSize(MSize mSize)
        {
          if (_context.MSizes == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.MSizes'  is null.");
          }
            _context.MSizes.Add(mSize);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMSize", new { id = mSize.SizeId }, mSize);
        }

        // DELETE: api/MSizes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMSize(int id)
        {
            if (_context.MSizes == null)
            {
                return NotFound();
            }
            var mSize = await _context.MSizes.FindAsync(id);
            if (mSize == null)
            {
                return NotFound();
            }

            _context.MSizes.Remove(mSize);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MSizeExists(int id)
        {
            return (_context.MSizes?.Any(e => e.SizeId == id)).GetValueOrDefault();
        }
    }
}
