using Jaahub.Data;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertyImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/propertyimages/{propertyId}
        [HttpGet("{propertyId}")]
        public async Task<ActionResult<IEnumerable<PropertyImage>>> GetPropertyImages(Guid propertyId)
        {
            var images = await _context.PropertyImages.Where(img => img.PropertyId == propertyId).ToListAsync();
            return Ok(images);
        }

        // POST: api/propertyimages
        [HttpPost]
        public async Task<ActionResult<PropertyImage>> PostPropertyImage(PropertyImage propertyImage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var property = await _context.Properties.FindAsync(propertyImage.PropertyId);
            if (property == null || property.OwnerId != Guid.Parse(userId))
            {
                return Forbid();
            }

            _context.PropertyImages.Add(propertyImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPropertyImages", new { propertyId = propertyImage.PropertyId }, propertyImage);
        }

        // DELETE: api/propertyimages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyImage(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var propertyImage = await _context.PropertyImages.Include(pi => pi.Property).FirstOrDefaultAsync(pi => pi.Id == id);
            if (propertyImage == null || propertyImage.Property.OwnerId != Guid.Parse(userId))
            {
                return Forbid();
            }

            _context.PropertyImages.Remove(propertyImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
