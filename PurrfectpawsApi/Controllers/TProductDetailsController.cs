using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
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
        public IConfiguration _configuration;
        private readonly PurrfectpawsContext _context;

        public TProductDetailsController(IConfiguration config, PurrfectpawsContext context)
        {
            _configuration = config;
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
                    (product, size) => new SizeDetailsDto
                    {
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
        public async Task<IActionResult> PutTProductDetail(int id, [FromForm] TPutProductDetail tProductDetail)
        {
            var existingProductDetail = await _context.TProductDetails.FindAsync(id);
            if (existingProductDetail == null)
            {
                return NotFound(); // Return 404 if the entity with the given 'id' is not found
            }

            existingProductDetail.CategoryId = tProductDetail.CategoryId;
            existingProductDetail.ProductName = tProductDetail.ProductName;
            existingProductDetail.ProductDescription = tProductDetail.ProductDescription;
            existingProductDetail.ProductPrice = tProductDetail.ProductPrice;
            existingProductDetail.ProductCost = tProductDetail.ProductCost;
            existingProductDetail.ProductRevenue = tProductDetail.ProductRevenue;
            existingProductDetail.ProductProfit = tProductDetail.ProductProfit;
            existingProductDetail.QuantitySold = tProductDetail.QuantitySold;

            if (tProductDetail.Images != null && tProductDetail.Images.Count > 0)
            {
                string connectionString = _configuration.GetConnectionString("PurrfectpawsBlobStorageConnString");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                foreach (var image in tProductDetail.Images)
                {
                    if (image.Length == 0)
                    {
                        continue;
                    }

                    var containerName = "storagecontainerpurrfectpaws";
                    var filename = Guid.NewGuid() + Path.GetExtension(image.FileName);

                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                    BlobClient blobClient = containerClient.GetBlobClient(filename);

                    using (var stream = image.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    var productBlobImage = new TProductBlobImage
                    {
                        ProductDetailsId = existingProductDetail.ProductDetailsId,
                        BlobStorageId = filename
                    };

                    _context.TProductBlobImages.Add(productBlobImage);
                }
            }

            // _context.Entry(tProductDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok("success");
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

            //return NoContent();
        }




        // POST: api/TProductDetails
        [HttpPost]
        public async Task<ActionResult<TPostProductDetail>> PostTProductDetail([FromForm] TPostProductDetail TPostProductDetail)
        {
            if (_context.TProductDetails == null)
            {
                return Problem("Entity set 'PurrfectpawsContext.TProductDetails'  is null.");
            }

            var newProductDetail = new TProductDetail
            {
                CategoryId = TPostProductDetail.CategoryId,
                ProductName = TPostProductDetail.ProductName,
                ProductDescription = TPostProductDetail.ProductDescription,
                ProductPrice = TPostProductDetail.ProductPrice,
                ProductCost = TPostProductDetail.ProductCost,
                ProductRevenue = TPostProductDetail.ProductRevenue,
                ProductProfit = TPostProductDetail.ProductProfit,
                QuantitySold = TPostProductDetail.QuantitySold
            };

            // newProductDetail.TProductBlobImages = tProductDetail.TProductBlobImages;

            _context.TProductDetails.Add(newProductDetail);

            await _context.SaveChangesAsync();

            if (TPostProductDetail.Images != null && TPostProductDetail.Images.Count > 0)
            {
                var uploadedFileNames = new List<string>();
                var imageUrls = new List<string>();

                string connectionString = _configuration.GetConnectionString("PurrfectpawsBlobStorageConnString");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                foreach (var image in TPostProductDetail.Images)
                {
                    if (image.Length == 0)
                    {
                        continue;
                    }

                    var containerName = "storagecontainerpurrfectpaws";
                    var filename = Guid.NewGuid() + Path.GetExtension(image.FileName);

                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                    BlobClient blobClient = containerClient.GetBlobClient(filename);

                    using (var stream = image.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    uploadedFileNames.Add(filename);

                    var imageUrl = $"{blobClient.Uri}";
                    imageUrls.Add(imageUrl);

                    var productBlobImage = new TProductBlobImage
                    {
                        ProductDetailsId = newProductDetail.ProductDetailsId,
                        BlobStorageId = filename
                    };

                    _context.TProductBlobImages.Add(productBlobImage);
                    await _context.SaveChangesAsync();


                }

            }


            //return CreatedAtAction("GetTProductDetail", new { id = tProductDetail.ProductDetailsId }, tProductDetail);

            return Ok("success");
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

            var tProduct = _context.TProducts.Where(c => c.ProductDetailsId == id).ToList();

            if (tProduct != null)
            {
                foreach (var product in tProduct)
                {
                    var TProduct = await _context.TProducts.FindAsync(product.ProductId);

                    _context.TProducts.Remove(TProduct);

                }
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