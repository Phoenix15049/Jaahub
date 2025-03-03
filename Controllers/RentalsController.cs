using Jaahub.Data;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            return await _context.Rentals.Where(r => r.RenterId == Guid.Parse(userId))
                                          .Include(r => r.Property)
                                          .ToListAsync();
        }

        // GET: api/rentals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(Guid id)
        {
            var rental = await _context.Rentals.Include(r => r.Property)
                                                .FirstOrDefaultAsync(r => r.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }

        // POST: api/rentals
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            rental.RenterId = Guid.Parse(userId);
            rental.CreatedAt = DateTime.UtcNow;
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRental", new { id = rental.Id }, rental);
        }
    }
}
