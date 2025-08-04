using Jaahub.Data;
using Jaahub.Dtos.Properties;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            return await _context.Properties.Include(p => p.Owner).Include(p => p.Category).ToListAsync();
        }

        // GET: api/properties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid id)
        {
            var property = await _context.Properties.Include(p => p.Owner).Include(p => p.Category)
                                                     .FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            return property;
        }

        // POST: api/properties
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyDto dto)
        {
            var owner = await _context.Users.FindAsync(dto.OwnerId);
            var category = await _context.Categories.FindAsync(dto.CategoryId);

            if (owner == null || category == null)
                return BadRequest("Invalid OwnerId or CategoryId");

            var property = new Property
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Price = (decimal)dto.Price,
                PropertyType = dto.PropertyType,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                SquareMeters = dto.SquareMeters,
                OwnerId = dto.OwnerId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return Ok(property);
        }

        // PUT: api/properties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProperty(Guid id, Property property)
        {
            if (id != property.Id)
            {
                return BadRequest();
            }

            _context.Entry(property).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/properties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
