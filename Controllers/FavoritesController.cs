using Jaahub.Data;
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
        public async Task<ActionResult<Favorite>> PostFavorite(Favorite favorite)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            favorite.UserId = Guid.Parse(userId);
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavorites", new { id = favorite.Id }, favorite);
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
