using Jaahub.Data;
using Jaahub.Dtos.Messages;
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
        public async Task<IActionResult> Create([FromBody] CreateMessageDto dto)
        {
            var sender = await _context.Users.FindAsync(dto.SenderId);
            var receiver = await _context.Users.FindAsync(dto.ReceiverId);
            var property = await _context.Properties.FindAsync(dto.PropertyId);

            if (sender == null || receiver == null || property == null)
                return BadRequest("Invalid SenderId, ReceiverId, or PropertyId");

            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                PropertyId = dto.PropertyId,
                MessageText = dto.MessageText,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }
    }
}
