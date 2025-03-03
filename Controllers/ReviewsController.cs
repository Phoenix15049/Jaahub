using Jaahub.Data;
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
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            review.UserId = Guid.Parse(userId);
            review.CreatedAt = DateTime.UtcNow;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviews", new { propertyId = review.PropertyId }, review);
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
