using Jaahub.Data;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyViewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertyViewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/propertyviews/{propertyId}
        [HttpGet("{propertyId}")]
        public async Task<ActionResult<IEnumerable<PropertyView>>> GetPropertyViews(Guid propertyId)
        {
            return await _context.PropertyViews.Where(pv => pv.PropertyId == propertyId).ToListAsync();
        }

        // POST: api/propertyviews
        [HttpPost]
        public async Task<ActionResult<PropertyView>> PostPropertyView(PropertyView propertyView)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            propertyView.UserId = Guid.Parse(userId);
            propertyView.ViewedAt = DateTime.UtcNow;
            _context.PropertyViews.Add(propertyView);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPropertyViews", new { propertyId = propertyView.PropertyId }, propertyView);
        }

        // DELETE: api/propertyviews/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyView(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var propertyView = await _context.PropertyViews.FindAsync(id);
            if (propertyView == null || propertyView.UserId != Guid.Parse(userId))
            {
                return NotFound();
            }

            _context.PropertyViews.Remove(propertyView);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
