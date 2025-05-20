namespace Messaging.Core.Commands
{
    public record UpdateOrderStatus
    {
        public Guid OrderId { get; init; }
        public DateTime DispatchDateTime { get; init; }
    }
}