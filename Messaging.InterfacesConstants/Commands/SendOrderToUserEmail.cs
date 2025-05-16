namespace Messaging.Core.Commands
{
    public record SendOrderToUserEmail
    {
        public Guid OrderId { get; init; }
        public string PictureUrl { get; init; }
        public string UserEmail { get; init; }
        public List<byte[]> Faces { get; init; }
    }
}