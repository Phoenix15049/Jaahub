using Jaahub.Data;
using Jaahub.Dtos.Favorites;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/favorites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            return await _context.Favorites.Where(f => f.UserId == Guid.Parse(userId))
                                            .Include(f => f.Property)
                                            .ToListAsync();
        }

        // POST: api/favorites
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFavoriteDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            var property = await _context.Properties.FindAsync(dto.PropertyId);

            if (user == null || property == null)
                return BadRequest("Invalid UserId or PropertyId");

            var favorite = new Favorite
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                PropertyId = dto.PropertyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok(favorite);
        }

        // DELETE: api/favorites/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null || favorite.UserId != Guid.Parse(userId))
            {
                return NotFound();
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
