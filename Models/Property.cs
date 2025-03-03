using Jaahub.Models;

namespace Jaahub.Models
{
    public class Property
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string PropertyType { get; set; } = "Apartment";
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? SquareMeters { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
