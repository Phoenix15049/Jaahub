using Jaahub.Models;

namespace Jaahub.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? PropertyId { get; set; } // در صورت حذف، مقدار NULL می‌شود
        public Property? Property { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; } // بین 1 تا 5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
