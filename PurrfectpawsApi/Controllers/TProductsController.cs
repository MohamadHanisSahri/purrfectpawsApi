using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
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
    public class TProductsController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TProductsController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProductsDto>>> GetTProducts()
        {
          if (_context.TProducts == null)
          {
              return NotFound();
          }

            var productList = await _context.TProducts
                   .Where(p => !p.IsDeleted) // Exclude soft deleted records
                  .Include(p => p.ProductDetails)
                  .ThenInclude(p => p.TProductBlobImages)
                  .Include(p => p.ProductDetails)
                  .ThenInclude(p => p.Category)
                  .Select(p => new TProductsDto
                  {
                      ProductId = p.ProductId,
                      ProductDetailsId = p.ProductDetailsId,
                      ProductName = p.ProductDetails.ProductName,
                      ProductDescription = p.ProductDetails.ProductDescription,
                      ProductPrice = p.ProductDetails.ProductPrice,
                      ProductCategoryId = p.ProductDetails.Category.CategoryId,
                      ProductCategory = p.ProductDetails.Category.Category,
                      ProductVariation = p.Variation.VariationName,
                      ProductSize = p.Size.SizeLabel,
                      ProductLength = p.LeadLength.LeadLength,
                      StockQuantity = p.ProductQuantity,
                      ProductImages = p.ProductDetails.TProductBlobImages
                          .Select(i => new TProductImagesDto
                          {
                              ImagesId = i.ProductImageId,
                              BlobImageUrl = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId
                          })
                          .ToList()

                  })
                .ToListAsync();

            return Ok(productList);
        }

        // GET: api/TProducts
        [HttpGet("Pagination/{pageNumber}/{itemsPerpage}")]
        public async Task<ActionResult<IEnumerable<TProductsDto>>> GetTProductsPagination(int pageNumber, int itemsPerPage)
        {
            if (_context.TProducts == null)
            {
                return NotFound();
            }

            if (pageNumber == null || itemsPerPage == null)
            {
                return BadRequest("Page number or items per page are not provided");
            }

            int recordsToSkip = (pageNumber - 1) * itemsPerPage;

            var productList = await _context.TProducts
                  .Include(p => p.ProductDetails)
                  .ThenInclude(p => p.TProductBlobImages)
                  .Include(p => p.ProductDetails)
                  .ThenInclude(p => p.Category)
                  .Select(p => new TProductsDto
                  {
                      ProductId = p.ProductId,
                      ProductDetailsId = p.ProductDetailsId,
                      ProductName = p.ProductDetails.ProductName,
                      ProductDescription = p.ProductDetails.ProductDescription,
                      ProductPrice = p.ProductDetails.ProductPrice,
                      ProductCategoryId = p.ProductDetails.Category.CategoryId,
                      ProductCategory = p.ProductDetails.Category.Category,
                      ProductVariation = p.Variation.VariationName,
                      ProductSize = p.Size.SizeLabel,
                      ProductLength = p.LeadLength.LeadLength,
                      StockQuantity = p.ProductQuantity,
                      ProductImages = p.ProductDetails.TProductBlobImages
                          .Select(i => new TProductImagesDto
                          {
                              ImagesId = i.ProductImageId,
                              BlobImageUrl = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId
                          })
                          .ToList()

                  })
                  .Skip(recordsToSkip)
                  .Take(itemsPerPage)
                .ToListAsync();

            return Ok(productList);
        }

        // GET: api/TProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TProduct>> GetTProduct(int id)
        {
          if (_context.TProducts == null)
          {
              return NotFound();
          }

            //var tProduct = await _context.TProducts.FindAsync(id);

            var tProduct = await _context.TProducts.
                            Include(p => p.ProductDetails).
                            Include(p => p.Size).
                            Include(p => p.LeadLength).
                            Include(p => p.Variation).
                            Where(p => !p.IsDeleted). // Exclude soft deleted records
                            FirstOrDefaultAsync( p => p.ProductId == id);


            if (tProduct == null)
            {
                return NotFound();
            }

            return tProduct;
        }

        // PUT: api/TProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTProduct(int id,[FromBody] TProductViewModel productViewModel)
        {
            if (id != productViewModel.ProductId)
            {
                return BadRequest("Invalid product id");
            }

            if (productViewModel.ProductDetailsId <= 0)      //TODO : temporary , try use data anotation but doesnt work
            {
                return BadRequest("ProductDetailsId is required");
            }

            var existingProduct = await _context.TProducts.FindAsync(id);

            if (existingProduct == null) return NotFound("Product not found");


            existingProduct.ProductDetailsId = productViewModel.ProductDetailsId;
            existingProduct.SizeId = productViewModel.SizeId;
            existingProduct.LeadLengthId = productViewModel.LeadLengthId;
            existingProduct.VariationId = productViewModel.VariationId;
            existingProduct.ProductQuantity = productViewModel.ProductQuantity;


            try
            {
                await _context.SaveChangesAsync();
                return Ok("Product updated successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
        }

        // POST: api/TProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TProduct>> PostTProduct(TProductViewModel productViewModel)
        {
          if (_context.TProducts == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.TProducts'  is null.");
          }

           if (productViewModel.ProductDetailsId <= 0)      //TODO : temporary , try use data anotation but doesnt work
           {
                return BadRequest("ProductDetailsId is required");
           }

            var tProduct = new TProduct
            {
                ProductDetailsId = productViewModel.ProductDetailsId,
                SizeId = productViewModel.SizeId,
                LeadLengthId = productViewModel.LeadLengthId,
                VariationId = productViewModel.VariationId,
                ProductQuantity = productViewModel.ProductQuantity
            };

            _context.TProducts.Add(tProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTProduct", new { id = tProduct.ProductId }, tProduct);
        }

        // DELETE: api/TProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTProduct(int id)
        {
            if (_context.TProducts == null)
            {
                return NotFound();
            }
            var tProduct = await _context.TProducts.FindAsync(id);
            if (tProduct == null)
            {
                return NotFound();
            }

            _context.TProducts.Remove(tProduct);
            await _context.SaveChangesAsync();

            return Ok("Product deleted successfully");
        }


        [HttpGet("getProductWithDetails/{id}")]
        public async Task<ActionResult<List<TProduct>>> GetTProductWithDetails(int id)
        {
            if (_context.TProducts == null)
            {
                return NotFound();
            }

            //var tProduct = await _context.TProducts.
            //                Include(p => p.ProductDetails).
            //                Include(p => p.Size).
            //                Include(p => p.LeadLength).
            //                Include(p => p.Variation).
            //                Where(p => p.ProductDetailsId == id).
            //                ToListAsync();    
            
            var tProduct = await _context.TProducts.
                            Select(p => new TProduct
                            {
                                ProductId = p.ProductId,
                                ProductDetailsId = p.ProductDetailsId,
                                SizeId = p.SizeId,
                                LeadLengthId = p.LeadLengthId,
                                VariationId = p.VariationId,
                                ProductQuantity = p.ProductQuantity,
                               // ProductDetails = p.ProductDetails,
                                Size = p.Size,
                                LeadLength = p.LeadLength,
                                Variation = p.Variation
      
                            }).
                            Where(p => p.ProductDetailsId == id).
                            ToListAsync();


            if (tProduct == null)
            {
                return NotFound();
            }

            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve, // Ignore circular references
            //    WriteIndented = true, // Indent the JSON output
            //};
            //var json = JsonSerializer.Serialize(tProduct, options);

            //return Content(json, "application/json");

            return tProduct;

        }

        private bool TProductExists(int id)
        {
            return (_context.TProducts?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
