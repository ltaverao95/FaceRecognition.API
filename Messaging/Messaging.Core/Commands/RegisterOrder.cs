namespace Messaging.Core.Commands
{
    public record RegisterOrder
    {
        public Guid OrderId { get; init;  }
        public string PictureUrl { get; init; }
        public string UserEmail { get; init; }
        public byte[] ImageData { get; init; }
    }
}