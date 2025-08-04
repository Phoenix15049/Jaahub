namespace Jaahub.Dtos.Properties
{
    public class CreatePropertyDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string PropertyType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
        public string Country { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? SquareMeters { get; set; }

        public Guid OwnerId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
