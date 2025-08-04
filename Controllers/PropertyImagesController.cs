using Jaahub.Data;
using Jaahub.Dtos.PropertyImages;
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
        public async Task<IActionResult> Create([FromBody] CreatePropertyImageDto dto)
        {
            var property = await _context.Properties.FindAsync(dto.PropertyId);
            if (property == null)
                return BadRequest("Invalid PropertyId");

            var image = new PropertyImage
            {
                Id = Guid.NewGuid(),
                PropertyId = dto.PropertyId,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.PropertyImages.Add(image);
            await _context.SaveChangesAsync();

            return Ok(image);
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
