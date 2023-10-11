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

        [HttpGet("DetailList/{productDetailsId}")]
        public async Task<ActionResult<TProductListByProductDetailsIdDto>> GetProductDetails(int productDetailsId)
        {
            try
            {
                // Query your database using Entity Framework to get product details
                var productDetails = await _context.TProductDetails
                    .Include(pd => pd.TProductBlobImages)
                    .Where(pd => pd.ProductDetailsId == productDetailsId)
                    .FirstOrDefaultAsync();

                if (productDetails == null)
                {
                    return NotFound("Product details not found");
                }

                // Query your database to get product variations and size details
                var productVariations = await _context.TProducts
                    .Where(p => p.ProductDetailsId == productDetailsId)
                    .Select(p => new TDetailsDto
                    {
                        ProductId = p.ProductId,
                        VariationId = p.VariationId,
                        VariationName = p.Variation.VariationName,
                        SizeId = p.SizeId,
                        SizeLabel = p.Size.SizeLabel,
                        LeadLengthId = p.LeadLengthId,
                        LeadLength = p.LeadLength.LeadLength,
                        ProductQuantity = p.ProductQuantity
                    })
                    .ToListAsync();

                // Shape the result into the desired data structure
                var productDetailsDto = new TProductListByProductDetailsIdDto
                {
                    ProductDetailsId = productDetails.ProductDetailsId,
                    ProductName = productDetails.ProductName,
                    ProductDescription = productDetails.ProductDescription,
                    ProductPrice = productDetails.ProductPrice,
                    Images = productDetails.TProductBlobImages.Select(image => new ImageDetailsDto
                    {
                        ProductImageId = image.ProductImageId,
                        BlobStorageId = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + image.BlobStorageId
                    }).ToList(),
                    TDetails = productVariations
                };

                return Ok(productDetailsDto);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log, return error response)
                return StatusCode(500, "An error occurred while fetching product details.");
            }
        }

        [HttpPut("UpdateProductDetails")]
        public async Task<IActionResult> PutProductDetails(TUpdateProductDetailsDto tUpdateProductDetailsDto)
        {
            Console.WriteLine(tUpdateProductDetailsDto);
            try
            {
                // Query your database using Entity Framework to get product details
                var getDetails = await _context.TProductDetails
                    .Include(pd => pd.TProductBlobImages)
                    .Where(pd => pd.ProductDetailsId == tUpdateProductDetailsDto.ProductDetailsId)
                    .FirstOrDefaultAsync();

                if (tUpdateProductDetailsDto == null)
                {
                    return NotFound("Product details not found");
                }

                // Query your database to get product variations and size details
                var productVariations = await _context.TProducts
                    .Where(p => p.ProductDetailsId == tUpdateProductDetailsDto.ProductDetailsId)
                    .Select(p => new TDetailsDto
                    {
                        ProductId = p.ProductId,
                        VariationId = p.VariationId,
                        VariationName = p.Variation.VariationName,
                        SizeId = p.SizeId,
                        SizeLabel = p.Size.SizeLabel,
                        LeadLengthId = p.LeadLengthId,
                        LeadLength = p.LeadLength.LeadLength,
                        ProductQuantity = p.ProductQuantity
                    })
                    .ToListAsync();

                // Shape the result into the desired data structure
                var productDetailsDto = new TProductListByProductDetailsIdDto
                {
                    ProductDetailsId = getDetails.ProductDetailsId,
                    ProductName = getDetails.ProductName,
                    ProductDescription = getDetails.ProductDescription,
                    ProductPrice = getDetails.ProductPrice,
                    TDetails = productVariations
                };

                var itemsToRemove = new List<TProduct>();

                foreach (var item in productDetailsDto.TDetails)
                {
                    if (!tUpdateProductDetailsDto.TDetails.Any(x => x.ProductId == item.ProductId || item.ProductId == null))
                    {
                        var getProduct = await _context.TProducts.FindAsync(item.ProductId);

                        _context.TProducts.Remove(getProduct);
                        await _context.SaveChangesAsync();

                    }
                }

                foreach (var item in tUpdateProductDetailsDto.TDetails)
                {
                    if (item.ProductId == null)
                    {
                        if (item.SizeLabel != null)
                        {
                            var sizeId = await _context.MSizes
                                  .Where(p => p.SizeLabel == item.SizeLabel)
                                  .FirstOrDefaultAsync();

                            if (sizeId == null)
                            {
                                return NotFound("Size does not exist!");
                            }
                            var variationId = await _context.TVariations
                                  .Where(p => p.VariationName == item.VariationName)
                                  .FirstOrDefaultAsync();

                            if (variationId == null)
                            {
                                return NotFound("Variation does not exist!");
                            }
                            var tProduct = new TProduct
                            {
                                ProductDetailsId = tUpdateProductDetailsDto.ProductDetailsId,
                                SizeId = sizeId?.SizeId ?? 0,
                                LeadLengthId = null,
                                VariationId = variationId?.VariationId ?? 0,
                                ProductQuantity = item.ProductQuantity
                            };

                            Console.WriteLine($"ProductDetailsId: {tProduct.ProductDetailsId}, SizeId: {tProduct.SizeId}, LeadLengthId: {tProduct.LeadLengthId}, VariationId: {tProduct.VariationId}, ProductQuantity: {tProduct.ProductQuantity}");

                            _context.TProducts.Add(tProduct);
                            await _context.SaveChangesAsync();
                        }
                        else if (item.LeadLength != null)
                        {
                            var leadLengthId = await _context.TLeadLengths
                                  .Where(p => p.LeadLength == item.LeadLength)
                                  .FirstOrDefaultAsync();

                            if (leadLengthId == null)
                            {
                                return NotFound("LeadLength does not exist!");
                            }
                            var variationId = await _context.TVariations
                                  .Where(p => p.VariationName == item.VariationName)
                                  .FirstOrDefaultAsync();

                            if (variationId == null)
                            {
                                return NotFound("Variation does not exist!");
                            }
                            var tProduct = new TProduct
                            {
                                ProductDetailsId = tUpdateProductDetailsDto.ProductDetailsId,
                                SizeId = null,
                                LeadLengthId = leadLengthId?.LeadLengthId ?? 0,
                                VariationId = variationId?.VariationId ?? 0,
                                ProductQuantity = item.ProductQuantity
                            };

                            _context.TProducts.Add(tProduct);
                            await _context.SaveChangesAsync();

                        }


                    }
                }

                var getProductDetails = await _context.TProductDetails
                    .Where(p => p.ProductDetailsId == tUpdateProductDetailsDto.ProductDetailsId)
                    .FirstOrDefaultAsync();

                if (getProductDetails == null)
                {
                    return NotFound("Product details not found!");
                }
                getProductDetails.ProductDescription = tUpdateProductDetailsDto.ProductDescription;
                getProductDetails.ProductName = tUpdateProductDetailsDto.ProductName;
                getProductDetails.ProductPrice = tUpdateProductDetailsDto.ProductPrice;


                await _context.SaveChangesAsync();
                return Ok("Update success");

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log, return error response)
                return StatusCode(500, "An error occurred while fetching product details.");
            }
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
                            FirstOrDefaultAsync(p => p.ProductId == id);


            if (tProduct == null)
            {
                return NotFound();
            }

            return tProduct;
        }

        // PUT: api/TProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTProduct(int id, [FromBody] TProductViewModel productViewModel)
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