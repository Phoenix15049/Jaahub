namespace Jaahub.Dtos.Rentals
{
    public class CreateRentalDto
    {
        public Guid PropertyId { get; set; }
        public Guid RenterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public double TotalPrice { get; set; }
    }
}
