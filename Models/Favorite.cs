using Jaahub.Models;

namespace Jaahub.Models
{
    public class Favorite
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
