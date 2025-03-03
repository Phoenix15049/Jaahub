using Jaahub.Models;

namespace Jaahub.Models
{
    public class PropertyView
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? PropertyId { get; set; } 
        public Property? Property { get; set; }
        public Guid? UserId { get; set; } 
        public User? User { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
