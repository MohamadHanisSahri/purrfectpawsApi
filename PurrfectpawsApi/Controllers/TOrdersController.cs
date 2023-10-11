using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using PurrfectpawsApi.DatabaseDbContext;
using PurrfectpawsApi.Models;
using System.Drawing;

namespace PurrfectpawsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TOrdersController : ControllerBase
    {
        private readonly PurrfectpawsContext _context;

        public TOrdersController(PurrfectpawsContext context)
        {
            _context = context;
        }

        // GET: api/TOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTOrder>>> GetTOrders()
        {
            if (_context.TOrders == null)
            {
                return NotFound();
            }

            var tOrder = await _context.TOrders.
                Select(o => new GetTOrder
                {
                    OrderId = o.OrderId,
                    OrderStatusId = o.OrderStatusId,
                    ProductId = o.ProductId,
                    ShippingAddressId = o.ShippingAddressId,
                    BillingAddressId = o.BillingAddressId,
                    OrderMasterId = o.OrderMasterId,
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,


                    OrderStatus = o.OrderStatus,
                    OrderMaster = o.OrderMaster,
                    BillingAddress = o.BillingAddress,
                    ShippingAddress = o.ShippingAddress,

                    ProductOrderDetailsDTO = new ProductOrderDetailsDTO
                    {
                        Product = new TProductOrderDTO
                        {
                            ProductId = o.Product.ProductId,
                            ProductQuantity = o.Product.ProductQuantity,
                            Size = o.Product.Size,
                            LeadLength = o.Product.LeadLength,
                            Variation = o.Product.Variation,
                        },
                        ProductDetails = o.Product.ProductDetails,
                        Images = _context.TProductBlobImages
                                                .Where(i => i.ProductDetailsId == o.Product.ProductDetailsId)
                                                .Select(i => new ImageDetailsDto
                                                {
                                                    ProductImageId = i.ProductImageId,
                                                    ProductDetailsId = i.ProductDetailsId,
                                                    BlobStorageId = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId,
                                                })
                                                .ToList()
                    }


                }).
                ToListAsync();



            return tOrder;


        }

        // GET: api/TOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTOrder>> GetTOrder(int id)
        {
            if (_context.TOrders == null)
            {
                return NotFound();
            }

            var tOrder = await _context.TOrders.Where(o => o.OrderId == id).
                                Select(o => new GetTOrder
                                {
                                    OrderStatusId = o.OrderStatusId,
                                    ProductId = o.ProductId,
                                    ShippingAddressId = o.ShippingAddressId,
                                    BillingAddressId = o.BillingAddressId,
                                    OrderMasterId = o.OrderMasterId,
                                    Quantity = o.Quantity,
                                    TotalPrice = o.TotalPrice,


                                    OrderStatus = o.OrderStatus,
                                    OrderMaster = o.OrderMaster,
                                    BillingAddress = o.BillingAddress,
                                    ShippingAddress = o.ShippingAddress,

                                    ProductOrderDetailsDTO = new ProductOrderDetailsDTO
                                    {
                                        Product = new TProductOrderDTO
                                        {
                                            ProductId = o.Product.ProductId,
                                            ProductQuantity = o.Product.ProductQuantity,
                                            Size = o.Product.Size,
                                            LeadLength = o.Product.LeadLength,
                                            Variation = o.Product.Variation,
                                        },
                                        ProductDetails = o.Product.ProductDetails,
                                        Images = _context.TProductBlobImages
                                                .Where(i => i.ProductDetailsId == o.Product.ProductDetailsId)
                                                .Select(i => new ImageDetailsDto
                                                {
                                                    ProductImageId = i.ProductImageId,
                                                    ProductDetailsId = i.ProductDetailsId,
                                                    BlobStorageId = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId,
                                                })
                                                .ToList()
                                    }


                                }).
                                FirstOrDefaultAsync();


            if (tOrder == null)
            {
                return NotFound();
            }

            return tOrder;
        }

        [HttpGet("GetOrderHistory/{userId}")]

        public async Task<ActionResult<IEnumerable<GetTOrder>>> GetOrderHistoryById(int userId)
        {
            if (_context.TOrders == null)
            {
                return NotFound();
            }

            List<int> orderMasterId =_context.MOrderMasters.Where(p => p.UserId == userId).Select(p => p.OrderMasterId).ToList();

            var tOrder = await _context.TOrders.
               Where(o => orderMasterId.Contains(o.OrderMaster.OrderMasterId)).
               Select(o => new GetTOrder
               {
                   OrderId = o.OrderId,
                   OrderStatusId = o.OrderStatusId,
                   ProductId = o.ProductId,
                   ShippingAddressId = o.ShippingAddressId,
                   BillingAddressId = o.BillingAddressId,
                   OrderMasterId = o.OrderMasterId,
                   Quantity = o.Quantity,
                   TotalPrice = o.TotalPrice,


                   OrderStatus = o.OrderStatus,
                   OrderMaster = o.OrderMaster,
                   BillingAddress = o.BillingAddress,
                   ShippingAddress = o.ShippingAddress,

                   ProductOrderDetailsDTO = new ProductOrderDetailsDTO
                   {
                       Product = new TProductOrderDTO
                       {
                           ProductId = o.Product.ProductId,
                           ProductQuantity = o.Product.ProductQuantity,
                           Size = o.Product.Size,
                           LeadLength = o.Product.LeadLength,
                           Variation = o.Product.Variation,
                       },
                       ProductDetails = o.Product.ProductDetails,
                       Images = _context.TProductBlobImages
                                               .Where(i => i.ProductDetailsId == o.Product.ProductDetailsId)
                                               .Select(i => new ImageDetailsDto
                                               {
                                                   ProductImageId = i.ProductImageId,
                                                   ProductDetailsId = i.ProductDetailsId,
                                                   BlobStorageId = "https://storagepurrfectpaws.blob.core.windows.net/storagecontainerpurrfectpaws/" + i.BlobStorageId,
                                               })
                                               .ToList()
                   }


               }).
               ToListAsync();

            return tOrder;


        }

        // PUT: api/TOrders/5
        [HttpPut("{id}/{orderStatusId}")]

        public async Task<IActionResult> PutTOrder(int id, int orderStatusId)
        {
            if (_context.TOrders == null) { return NotFound(); }

            var TOrders = await _context.TOrders.FindAsync(id);

            if (TOrders == null)
            {
                return NotFound();
            }

            TOrders.OrderStatusId = orderStatusId;

           await _context.SaveChangesAsync();

           return Ok("success");

        }

        // POST: api/TOrders
        [HttpPost]
        public async Task<ActionResult<TPostOrderDTO>> PostTOrder(TPostOrderDTO tOrderDTO)
        {
            if (_context.TOrders == null)
            {
                return Problem("Entity set 'PurrfectpawsContext.TOrders'  is null.");
            }

            if(tOrderDTO.PaymentStatusId == 2)
            {
                return Problem("Payment failed");

            }

            var cartList = _context.TCarts.Where(c => c.UserId == tOrderDTO.UserId).ToList();  

            if (cartList.Count == 0) { return BadRequest("Cart not found");  }

            var totalProduct = 00.00M;

            var orderMasterId = await CreateMasterOrder(tOrderDTO.UserId);

            foreach (var cart in cartList)
            {
                //### Calculation in cart
                var productDetails = await _context.TProducts.
                                Where(p => p.ProductId == cart.ProductId).
                                Select(p => new
                                {
                                    p.ProductDetails.ProductPrice,
                                    p.ProductQuantity
                                }).
                                FirstOrDefaultAsync();
                if (productDetails == null) return Problem("Product not found");

                //check and calculat product price 
                var productTotal = productDetails.ProductPrice * cart.Quantity;
                totalProduct += productTotal;

                //////////////////////////////////////////////////////////////

                 var existingProduct = await _context.TProducts.FindAsync(cart.ProductId);

                if (existingProduct == null) return NotFound("Product not found");

                //check & reduce quantity
                if (cart.Quantity > existingProduct.ProductQuantity)
                {
                    return Problem("Product quantity exceed from available stocks");
                }
                else
                {
                    existingProduct.ProductQuantity = existingProduct.ProductQuantity - cart.Quantity;

                }

                var tOrder = new TOrder
                {
                    // Associate the order with the existing product
                    Product = existingProduct,
                    ShippingAddressId = tOrderDTO.ShippingAddressId,
                    BillingAddressId = tOrderDTO.BillingAddressId,
                    OrderMasterId = orderMasterId,
                    OrderStatusId = 1,
                    Quantity = cart.Quantity,
                    TotalPrice = productTotal
                };

                _context.TOrders.Add(tOrder);

                //delete item from cart
                var removeCartItem = await _context.TCarts.FindAsync(cart.CartId);
                if (removeCartItem == null)
                {
                    return NotFound();
                }

                _context.TCarts.Remove(removeCartItem);

            }


            //check price total // add 5 for shipping fee
            if ((totalProduct+tOrderDTO.ShippingFee) != tOrderDTO.TotalPrice) { return Problem("Total price not same"); }


            //add transaction
            CreateTransaction(tOrderDTO.PaymentStatusId , orderMasterId , totalProduct );

            await _context.SaveChangesAsync();


            return Ok("Order created successfully");

        }

        // DELETE: api/TOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTOrder(int id)
        {
            if (_context.TOrders == null)
            {
                return NotFound();
            }
            var tOrder = await _context.TOrders.FindAsync(id);
            if (tOrder == null)
            {
                return NotFound();
            }

            _context.TOrders.Remove(tOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPut("OrderStatus/{orderMasterId}/{orderStatusId}")]
        public async Task<IActionResult> PutOrderStatus(int orderMasterId, int orderStatusId)
        {
            if (_context.TOrders == null) { return NotFound(); }

            var mOrderStatus = await _context.MOrderStatuses.FindAsync(orderStatusId);
            if (mOrderStatus == null)
            {
                return NotFound();
            }

            var tOrder =  _context.TOrders.Where(o => o.OrderMasterId == orderMasterId).ToList();
            if (tOrder.Count == 0) { return NotFound(); }

            foreach(var order in tOrder)
            {
                order.OrderStatusId = orderStatusId;

            }

            await _context.SaveChangesAsync();

            return Ok("success");
        }


        private bool TOrderExists(int id)
        {
            return (_context.TOrders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

        private async Task<int> CreateMasterOrder(int userID = 1)
        {

            var OrderMaster = new MOrderMaster { UserId = userID };

            _context.MOrderMasters.Add(OrderMaster);

            await _context.SaveChangesAsync();

            return OrderMaster.OrderMasterId;

        }


        private async void CreateTransaction( int paymentStatusId = 1, int orderMasterId = 1, decimal amount = 0 )
        {

            var TTransaction = new TTransaction
            {
                PaymentStatusId = paymentStatusId,
                OrderMasterId = orderMasterId,
                TransactionDate = DateTime.Now,
                TransactionAmount = amount
                
            };

             _context.TTransactions.Add(TTransaction);

        }


        private async Task<decimal> GetProductPrice(int productId)
        {
            var productDetails = await _context.TProducts.
                Where(p => p.ProductId == productId).
                Select(p => new
                {
                    p.ProductDetails.ProductPrice
                }).
                FirstOrDefaultAsync();

            return productDetails.ProductPrice;
        }
    }
}
