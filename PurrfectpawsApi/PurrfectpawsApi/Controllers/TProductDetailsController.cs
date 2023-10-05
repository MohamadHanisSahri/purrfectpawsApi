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
    public class TProductDetailsController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TProductDetailsController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TProductDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProductDetail>>> GetTProductDetails()
        {
          if (_context.TProductDetails == null)
          {
              return NotFound();
          }
            return await _context.TProductDetails.ToListAsync();
        }

        // GET: api/TProductDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TProductDetail>> GetTProductDetail(int id)
        {
          if (_context.TProductDetails == null)
          {
              return NotFound();
          }
            var tProductDetail = await _context.TProductDetails.FindAsync(id);

            if (tProductDetail == null)
            {
                return NotFound();
            }

            return tProductDetail;
        }

        // GET: api/TProductDetails/5
        [HttpGet("Quantity/{productDetailsId}/{sizeId}/{variationId}/{leadLengthId}")]
        public async Task<ActionResult<TProductDetailsQuantityDto>> GetTProductQuantity(int productDetailsId, int sizeId, int variationId, int leadLengthId)
        {
            Console.WriteLine("ProductDetailsId: " + productDetailsId + ", SizeId: " + sizeId + ", VariationId: " + variationId + ", LeadLengthId: " + leadLengthId);


            if (sizeId == 0 || sizeId == null)
            {
                var productSizeQuantity = await _context.TProducts
                    .FirstOrDefaultAsync(p => p.ProductDetailsId == productDetailsId && p.LeadLengthId == leadLengthId && p.VariationId == variationId);
                //var productSizeQuantity = _context.TProducts
                //    .Where(p => p.ProductDetailsId == productDetailsId && p.LeadLengthId == leadLengthId && p.VariationId == variationId)
                //    .ToListAsync();
                if (productSizeQuantity != null)
                {

                    var responseSizeQuantity = new TProductDetailsQuantityDto
                    {
                        Quantity = productSizeQuantity.ProductQuantity
                    };

                    return Ok(responseSizeQuantity);

                }
            }
            var productLengthQuantity = await _context.TProducts
                .FirstOrDefaultAsync(p => p.ProductDetailsId == productDetailsId && p.SizeId == sizeId && p.VariationId == variationId);
            //var productLengthQuantity = _context.TProducts
            //        .Where(p => p.ProductDetailsId == productDetailsId && p.SizeId == sizeId && p.VariationId == variationId)
            //        .ToListAsync();
            if (productLengthQuantity != null)
            {
                var responseLengthQuantity = new TProductDetailsQuantityDto
                {
                    ProductId = productLengthQuantity.ProductId,
                    Quantity = productLengthQuantity.ProductQuantity
                };

                return Ok(responseLengthQuantity);

            }
            return NotFound();
        }

        [HttpGet("Product/{productId}")]
        public async Task<ActionResult<TProductDetailsViewDto>> GetProductDetails(int productId)
        {
            var productDetails = await _context.TProducts
                .Include(d => d.ProductDetails)
                .FirstOrDefaultAsync(u => u.ProductDetailsId == productId);

            if (productDetails == null)
            {
                return NotFound("Product not found");
            }

            var sizeDetails = await _context.TProducts
                .Where(p => p.ProductDetailsId == productDetails.ProductDetailsId)
                .Join(
                    _context.MSizes,
                    product => product.SizeId,
                    size => size.SizeId,
                    (product, size) => new SizeDetailsDto {
                        SizeId = size.SizeId,
                        SizeLabel = size.SizeLabel,
                        ProductQuantity = product.ProductQuantity
                    })
                .GroupBy(s => s.SizeId)
                .Select(group => group.First())
                .ToListAsync();

            var variationDetails = await _context.TProducts
                .Where(p => p.ProductDetailsId == productDetails.ProductDetailsId)
                .Join(
                    _context.TVariations,
                    product => product.VariationId,
                    variation => variation.VariationId,
                    (product, variation) => new VariationDto
                    {
                        VariationId = variation.VariationId,
                        VariationName = variation.VariationName,
                        ProductQuantity = product.ProductQuantity
                    })
                .GroupBy(v => v.VariationId)
                .Select(group => group.First())
                .ToListAsync();

            var leadLengthDetails = await _context.TProducts
                .Where(p => p.ProductDetailsId == productDetails.ProductDetailsId)
                .Join(
                    _context.TLeadLengths,
                    product => product.LeadLengthId,
                    lead => lead.LeadLengthId,
                    (product, lead) => new LeadLengthDetailsDto
                    {
                        LeadLengthId = lead.LeadLengthId,
                        LeadLength = lead.LeadLength,
                        ProductQuantity = product.ProductQuantity
                    })
                .GroupBy(l => l.LeadLengthId)
                .Select(group => group.First())
                .ToListAsync();

            var imageDetails = await _context.TProductBlobImages.Where(i => i.ProductDetailsId == productDetails.ProductDetailsId).ToListAsync();

            var productResponse = new TProductDetailsViewDto
            {
                ProductId = productDetails.ProductId,
                ProductDetailsId = productDetails.ProductDetailsId,
                ProductDescription = productDetails.ProductDetails.ProductDescription,
                ProductName = productDetails.ProductDetails.ProductName,
                ProductPrice = productDetails.ProductDetails.ProductPrice,
                Images = imageDetails.Select(i => new ImageDetailsDto
                {
                    ProductImageId = i.ProductImageId,
                    ProductDetailsId = i.ProductDetailsId,
                    BlobStorageId = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId,
                }).ToList(),
                Sizes = sizeDetails.Select(d => new SizeDetailsDto
                {
                    SizeId = d.SizeId,
                    SizeLabel = d.SizeLabel,
                    ProductQuantity = d.ProductQuantity
                }).ToList(),
                LeadLengths = leadLengthDetails.Select(l => new LeadLengthDetailsDto
                {
                    LeadLengthId = l.LeadLengthId,
                    LeadLength = l.LeadLength,
                    ProductQuantity = l.ProductQuantity
                }).ToList(),
                Variations = variationDetails.Select(v => new VariationDto
                {
                    VariationId = v.VariationId,
                    VariationName = v.VariationName,
                    ProductQuantity = v.ProductQuantity
                }).ToList()
            };

            return Ok(productResponse);
        }

        // PUT: api/TProductDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTProductDetail(int id, TProductDetail tProductDetail)
        {
            if (id != tProductDetail.ProductDetailsId)
            {
                return BadRequest();
            }

            _context.Entry(tProductDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TProductDetailExists(id))
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

        // POST: api/TProductDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TProductDetail>> PostTProductDetail(TProductDetail tProductDetail)
        {
          if (_context.TProductDetails == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.TProductDetails'  is null.");
          }
            _context.TProductDetails.Add(tProductDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTProductDetail", new { id = tProductDetail.ProductDetailsId }, tProductDetail);
        }

        // DELETE: api/TProductDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTProductDetail(int id)
        {
            if (_context.TProductDetails == null)
            {
                return NotFound();
            }
            var tProductDetail = await _context.TProductDetails.FindAsync(id);
            if (tProductDetail == null)
            {
                return NotFound();
            }

            _context.TProductDetails.Remove(tProductDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TProductDetailExists(int id)
        {
            return (_context.TProductDetails?.Any(e => e.ProductDetailsId == id)).GetValueOrDefault();
        }
    }
}
