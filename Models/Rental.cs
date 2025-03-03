using Jaahub.Models;

namespace Jaahub.Models
{
    public class Rental
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? PropertyId { get; set; } 
        public Property? Property { get; set; }
        public Guid RenterId { get; set; }
        public User Renter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
