using Jaahub.Data;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User userUpdate)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // فقط ویژگی‌هایی که مقداردهی شده‌اند را به‌روزرسانی کن
            if (!string.IsNullOrEmpty(userUpdate.FullName))
                existingUser.FullName = userUpdate.FullName;
            if (!string.IsNullOrEmpty(userUpdate.Email))
                existingUser.Email = userUpdate.Email;
            if (!string.IsNullOrEmpty(userUpdate.PasswordHash))
                existingUser.PasswordHash = userUpdate.PasswordHash;
            if (!string.IsNullOrEmpty(userUpdate.PhoneNumber))
                existingUser.PhoneNumber = userUpdate.PhoneNumber;
            if (!string.IsNullOrEmpty(userUpdate.ProfileImageUrl))
                existingUser.ProfileImageUrl = userUpdate.ProfileImageUrl;
            if (!string.IsNullOrEmpty(userUpdate.Role))
                existingUser.Role = userUpdate.Role;

            // Id و CreatedAt تغییر نکنند
            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
