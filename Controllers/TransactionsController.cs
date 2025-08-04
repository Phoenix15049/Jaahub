using Jaahub.Data;
using Jaahub.Dtos.Transactions;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            return await _context.Transactions.Where(t => t.UserId == Guid.Parse(userId))
                                              .Include(t => t.Property)
                                              .Include(t => t.Rental)
                                              .ToListAsync();
        }

        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
        {
            var transaction = await _context.Transactions.Include(t => t.Property).Include(t => t.Rental)
                                                          .FirstOrDefaultAsync(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return BadRequest("Invalid UserId");

            Property property = null;
            if (dto.PropertyId.HasValue)
            {
                property = await _context.Properties.FindAsync(dto.PropertyId.Value);
                if (property == null)
                    return BadRequest("Invalid PropertyId");
            }

            Rental rental = null;
            if (dto.RentalId.HasValue)
            {
                rental = await _context.Rentals.FindAsync(dto.RentalId.Value);
                if (rental == null)
                    return BadRequest("Invalid RentalId");
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                PropertyId = dto.PropertyId,
                RentalId = dto.RentalId,
                Amount = (decimal)dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}
