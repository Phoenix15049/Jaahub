namespace Jaahub.Models
{
    public class PropertyImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
