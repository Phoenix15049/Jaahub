namespace Jaahub.Dtos.Favorites
{
    public class CreateFavoriteDto
    {
        public Guid UserId { get; set; }
        public Guid PropertyId { get; set; }
    }
}
