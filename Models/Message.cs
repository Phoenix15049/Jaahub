using Jaahub.Models;

namespace Jaahub.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? SenderId { get; set; }
        public User? Sender { get; set; }
        public Guid? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
