using Jaahub.Data;
using Jaahub.Dtos.Rentals;
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

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalDto dto)
        {
            var property = await _context.Properties.FindAsync(dto.PropertyId);
            var renter = await _context.Users.FindAsync(dto.RenterId);

            if (property == null || renter == null)
                return BadRequest("Invalid PropertyId or RenterId");

            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                RenterId = dto.RenterId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                TotalPrice = (decimal)dto.TotalPrice,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

    }
}
