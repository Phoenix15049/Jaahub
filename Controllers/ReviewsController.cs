using Jaahub.Data;
using Jaahub.Dtos.Reviews;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reviews/{propertyId}
        [HttpGet("{propertyId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(Guid propertyId)
        {
            return await _context.Reviews.Where(r => r.PropertyId == propertyId)
                                          .Include(r => r.User)
                                          .ToListAsync();
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            var property = await _context.Properties.FindAsync(dto.PropertyId);

            if (user == null || property == null)
                return BadRequest("Invalid UserId or PropertyId");

            var review = new Review
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null || review.UserId != Guid.Parse(userId))
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
