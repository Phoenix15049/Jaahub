namespace Jaahub.Dtos.Transactions
{
    public class CreateTransactionDto
    {
        public Guid UserId { get; set; }
        public Guid? PropertyId { get; set; }
        public Guid? RentalId { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
