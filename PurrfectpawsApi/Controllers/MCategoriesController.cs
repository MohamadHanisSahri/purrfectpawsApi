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
    public class MCategoriesController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public MCategoriesController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/MCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MCategory>>> GetMCategories()
        {
          if (_context.MCategories == null)
          {
              return NotFound();
          }
            return await _context.MCategories.ToListAsync();
        }

        // GET: api/MCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MCategory>> GetMCategory(int id)
        {
          if (_context.MCategories == null)
          {
              return NotFound();
          }
            var mCategory = await _context.MCategories.FindAsync(id);

            if (mCategory == null)
            {
                return NotFound();
            }

            return mCategory;
        }

        // PUT: api/MCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMCategory(int id, MCategory mCategory)
        {
            if (id != mCategory.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(mCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MCategoryExists(id))
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

        // POST: api/MCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MCategory>> PostMCategory(MCategory mCategory)
        {
          if (_context.MCategories == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.MCategories'  is null.");
          }
            _context.MCategories.Add(mCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMCategory", new { id = mCategory.CategoryId }, mCategory);
        }

        // DELETE: api/MCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMCategory(int id)
        {
            if (_context.MCategories == null)
            {
                return NotFound();
            }
            var mCategory = await _context.MCategories.FindAsync(id);
            if (mCategory == null)
            {
                return NotFound();
            }

            _context.MCategories.Remove(mCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MCategoryExists(int id)
        {
            return (_context.MCategories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
