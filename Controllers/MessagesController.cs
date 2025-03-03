using Jaahub.Data;
using Jaahub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jaahub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            return await _context.Messages.Where(m => m.SenderId == Guid.Parse(userId) || m.ReceiverId == Guid.Parse(userId))
                                           .Include(m => m.Sender)
                                           .Include(m => m.Receiver)
                                           .Include(m => m.Property)
                                           .ToListAsync();
        }

        // GET: api/messages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(Guid id)
        {
            var message = await _context.Messages.Include(m => m.Sender)
                                                 .Include(m => m.Receiver)
                                                 .Include(m => m.Property)
                                                 .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // POST: api/messages
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            message.SenderId = Guid.Parse(userId);
            message.CreatedAt = DateTime.UtcNow;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }
    }
}
