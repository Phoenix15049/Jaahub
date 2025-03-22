using Jaahub.Models;

namespace Jaahub.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? PropertyId { get; set; }
        public Property? Property { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
