namespace Jaahub.Dtos.Reviews
{
    public class CreateReviewDto
    {
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
