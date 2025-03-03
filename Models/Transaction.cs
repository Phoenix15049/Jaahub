using Jaahub.Models;

namespace Jaahub.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? PropertyId { get; set; }
        public Property? Property { get; set; }
        public Guid? RentalId { get; set; }
        public Rental? Rental { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Credit Card"; // Credit Card, PayPal, Crypto
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
