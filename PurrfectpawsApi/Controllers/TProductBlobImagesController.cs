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
    public class TProductBlobImagesController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TProductBlobImagesController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TProductBlobImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProductBlobImage>>> GetTProductBlobImages()
        {
          if (_context.TProductBlobImages == null)
          {
              return NotFound();
          }
            return await _context.TProductBlobImages.ToListAsync();
        }

        // GET: api/TProductBlobImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TProductBlobImage>> GetTProductBlobImage(int id)
        {
          if (_context.TProductBlobImages == null)
          {
              return NotFound();
          }
            var tProductBlobImage = await _context.TProductBlobImages.FindAsync(id);

            if (tProductBlobImage == null)
            {
                return NotFound();
            }

            return tProductBlobImage;
        }

        // PUT: api/TProductBlobImages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTProductBlobImage(int id, TProductBlobImage tProductBlobImage)
        {
            if (id != tProductBlobImage.ProductImageId)
            {
                return BadRequest();
            }

            _context.Entry(tProductBlobImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TProductBlobImageExists(id))
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

        // POST: api/TProductBlobImages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TProductBlobImage>> PostTProductBlobImage(TProductBlobImage tProductBlobImage)
        {
          if (_context.TProductBlobImages == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.TProductBlobImages'  is null.");
          }
            _context.TProductBlobImages.Add(tProductBlobImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTProductBlobImage", new { id = tProductBlobImage.ProductImageId }, tProductBlobImage);
        }

        // DELETE: api/TProductBlobImages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTProductBlobImage(int id)
        {
            if (_context.TProductBlobImages == null)
            {
                return NotFound();
            }
            var tProductBlobImage = await _context.TProductBlobImages.FindAsync(id);
            if (tProductBlobImage == null)
            {
                return NotFound();
            }

            _context.TProductBlobImages.Remove(tProductBlobImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TProductBlobImageExists(int id)
        {
            return (_context.TProductBlobImages?.Any(e => e.ProductImageId == id)).GetValueOrDefault();
        }
    }
}
