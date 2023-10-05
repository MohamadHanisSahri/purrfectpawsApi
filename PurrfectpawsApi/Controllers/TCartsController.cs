using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurrfectpawsApi.DatabaseDbContext;
using PurrfectpawsApi.Models;

namespace PurrfectpawsApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TCartsController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TCartsController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCart>>> GetTCarts()
        {
          if (_context.TCarts == null)
          {
              return NotFound();
          }
            return await _context.TCarts.ToListAsync();
        }

        // GET: api/TCarts/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<TCartListViewDto>> GetTCartList(int userId)
        {

            var userCart = await _context.TUsers
                .Include(u => u.TCarts)
                .ThenInclude(c => c.Product.Size)
                .Include(u => u.TCarts)
                .ThenInclude(c => c.Product.LeadLength)
                .Include(u => u.TCarts)
                .ThenInclude(c => c.Product.Variation)
                .Include(u => u.TCarts)
                .ThenInclude(c => c.Product.ProductDetails)
                .ThenInclude(c => c.TProductBlobImages)
                .Include(u => u.TShippingAddresses)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userCart == null)
            {
                return NotFound("User not found");
            }

            var cartList = new TCartListViewDto
            {
                UserId = userCart.UserId,
                UserName = userCart.Name,
                UserShippingAddress = userCart.TShippingAddresses.Select(s => new UserShippingAddressDto
                {
                    ShippingAdddressId = s.ShippingAddressId,
                    Street1 = s.Street1,
                    Street2 = s.Street2,
                    city = s.City,
                    State = s.State,
                    Postcode = s.Postcode,
                    Country = s.Country
                }).FirstOrDefault(),
                ProductDetails = new List<CartProductDetailsDto>()
            };

            foreach (var item in userCart.TCarts)
            {
                var productImages = await _context.TProductBlobImages
                    .Where(i => i.ProductDetailsId == item.Product.ProductDetailsId)
                    .Select(i => new TProductImageDto
                    {
                        ImageId = i.ProductImageId,
                        ImageUrl = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId
                    })
                    .ToListAsync();

                var productDetails = new CartProductDetailsDto
                {
                    ProductId = item.ProductId,
                    CartId = item.CartId,
                    ProductName = item.Product.ProductDetails?.ProductName,
                    ProductDescription = item.Product.ProductDetails?.ProductDescription,
                    ProductPrice = item.Product.ProductDetails?.ProductPrice ?? 0,
                    ProductSize = item.Product.Size?.SizeLabel,
                    ProductLength = item.Product.LeadLength?.LeadLength ?? 0,
                    ProductVariation = item.Product.Variation?.VariationName,
                    CartQuantity = item.Quantity,
                    TotalPrice = (decimal)(item.Product.ProductDetails?.ProductPrice * item.Quantity),
                    StockQuantity = item.Product.ProductQuantity,
                    ProductImages = productImages
                };

                cartList.ProductDetails.Add(productDetails);
            }
            return Ok(cartList);
        }

        // PUT: api/TCarts/EditCartQuantity/1/4
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditCartQuantity/{cartId}/{quantity}")]
        public async Task<IActionResult> PutTCart(int cartId, int quantity)
        {
            try
            {
                var isCartExist = _context.TCarts
                    .Where(c => c.CartId == cartId)
                    .FirstOrDefault();

                if (isCartExist == null)
                {
                    return NotFound("Cart not found");
                }

                isCartExist.Quantity = quantity;

                await _context.SaveChangesAsync();

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Update success");
        }

        // PUT: api/TCarts/IncrementCart/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("IncrementCart/{cartId}")]
        public async Task<IActionResult> PutIncrementTCart(int cartId)
        {
            try
            {
                var isCartExist = _context.TCarts
                    .Where(c => c.CartId == cartId)
                    .FirstOrDefault();

                if (isCartExist == null)
                {
                    return NotFound("Cart not found");
                }

                isCartExist.Quantity++;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Update success");
        }

        // PUT: api/DecrementCart/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("DecrementCart/{cartId}")]
        public async Task<IActionResult> PutDecrementTCart(int cartId)
        {
            try
            {
                var isCartExist = _context.TCarts
                    .Where(c => c.CartId == cartId)
                    .FirstOrDefault();

                if (isCartExist == null)
                {
                    return NotFound("Cart not found");
                }

                isCartExist.Quantity--;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Update success");
        }

        // POST: api/TCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TCart>> PostTCart([FromBody] TCartAddDto tCart)
        {
            if (_context.TCarts == null)
          {
              return Problem("Entity set 'PurrfectpawsContext.TCarts'  is null.");
          }

          var isCartListExist = _context.TCarts.FirstOrDefault(t => t.ProductId == tCart.ProductId && t.UserId == tCart.UserId);
            if (isCartListExist == null)
            {
                var newCart = new TCart
                {
                    ProductId = tCart.ProductId,
                    UserId = tCart.UserId,
                    Quantity = tCart.Quantity != null && tCart.Quantity != 0 ? tCart.Quantity : 1,
                };
                _context.TCarts.Add(newCart);
            } else
            {
                isCartListExist.Quantity++;
            }

            await _context.SaveChangesAsync();

            return Ok("Success");
        }

        // DELETE: api/TCarts/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteTCart(int productId)
        {
            if (_context.TCarts == null)
            {
                return NotFound();
            }
            var tCart = await _context.TCarts.FirstOrDefaultAsync(pid => pid.ProductId == productId);
            if (tCart == null)
            {
                return NotFound();
            }

            _context.TCarts.Remove(tCart);
            await _context.SaveChangesAsync();

            return Ok("Deleted success");
        }

        private bool TCartExists(int id)
        {
            return (_context.TCarts?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
