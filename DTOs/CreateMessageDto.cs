namespace Jaahub.Dtos.Messages
{
    public class CreateMessageDto
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid PropertyId { get; set; }
        public string MessageText { get; set; }
    }
}
