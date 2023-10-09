using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurrfectpawsApi.DatabaseDbContext;
using PurrfectpawsApi.Models;

namespace PurrfectpawsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TTransactionsController : ControllerBase
    {
 
        private readonly PurrfectpawsContext _context;

        public TTransactionsController(PurrfectpawsContext context )
        {
            _context = context;
        }

        // GET: api/TTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTTransaction>>> GetTTransactions()
        {
          if (_context.TTransactions == null)
          {
              return NotFound();
          }

          var transactions = await _context.TTransactions.
                Select( t => new GetTTransaction
                {
                    PaymentStatusId = t.PaymentStatusId,
                    OrderMasterId = t.OrderMasterId,
                    TransactionDate = t.TransactionDate,
                    TransactionAmount = t.TransactionAmount,
                    PaymentStatus = t.PaymentStatus,

                    TransactionOrderMaster = new TransactionOrderMaster
                    {
                        TOrders = t.OrderMaster.TOrders.Select(o => new GetTOrder
                        {
                            OrderId = o.OrderId,
                            OrderStatusId = o.OrderStatusId,
                            ProductId = o.ProductId,
                            Quantity = o.Quantity,
                            TotalPrice = o.TotalPrice,
                            OrderStatus = o.OrderStatus,
                            BillingAddress = o.BillingAddress,
                            ShippingAddress = o.ShippingAddress
 
                        }).ToList(),

                        User = new TUserGetsDTO
                        {
                            Name = t.OrderMaster.User.Name,
                            Email = t.OrderMaster.User.Email
                        }

                    }

                }).
                ToListAsync();

            return transactions;
        }

        // GET: api/TTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTTransaction>> GetTTransaction(int id)
        {
          if (_context.TTransactions == null)
          {
              return NotFound();
          }
            var transactions = await _context.TTransactions.Where(o => o.TransactionId == id).
                 Select(t => new GetTTransaction
                 {
                     PaymentStatusId = t.PaymentStatusId,
                     OrderMasterId = t.OrderMasterId,
                     TransactionDate = t.TransactionDate,
                     TransactionAmount = t.TransactionAmount,
                     PaymentStatus = t.PaymentStatus,

                     TransactionOrderMaster = new TransactionOrderMaster
                     {
                         TOrders = t.OrderMaster.TOrders.Select(o => new GetTOrder
                         {
                             OrderId = o.OrderId,
                             OrderStatusId = o.OrderStatusId,
                             ProductId = o.ProductId,
                             Quantity = o.Quantity,
                             TotalPrice = o.TotalPrice,
                             OrderStatus = o.OrderStatus,
                             BillingAddress = o.BillingAddress,
                             ShippingAddress = o.ShippingAddress

                         }).ToList(),

                         User = new TUserGetsDTO
                         {
                             Name = t.OrderMaster.User.Name,
                             Email = t.OrderMaster.User.Email
                         }

                     }

                 }).
                 FirstOrDefaultAsync();


            if (transactions == null)
            {
                return NotFound();
            }

            return transactions;
        }

        // PUT: api/TTransactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTTransaction(int id, TTransaction tTransaction)
        {
            if (id != tTransaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(tTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TTransactionExists(id))
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

        // POST: api/TTransactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TTransaction>> PostTTransaction(TPostTransaction tTransaction)
        {

              if (_context.TTransactions == null)
              {
                  return Problem("Entity set 'PurrfectpawsContext.TTransactions'  is null.");
              }

            var newTransaction = new TTransaction
            {
                PaymentStatusId = 1,
                OrderMasterId = 1,
                TransactionDate = DateTime.Now,
                TransactionAmount = 1
            };

            _context.TTransactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetTTransaction", new { id = tTransaction.TransactionId }, tTransaction);
            return Ok("success");
        }

        // DELETE: api/TTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTTransaction(int id)
        {
            if (_context.TTransactions == null)
            {
                return NotFound();
            }
            var tTransaction = await _context.TTransactions.FindAsync(id);
            if (tTransaction == null)
            {
                return NotFound();
            }

            _context.TTransactions.Remove(tTransaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TTransactionExists(int id)
        {
            return (_context.TTransactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
