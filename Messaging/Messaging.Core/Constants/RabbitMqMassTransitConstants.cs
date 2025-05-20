namespace Messaging.Core.Constants
{
    public class RabbitMqMassTransitConstants
    {
        //Commands
        public const string OrderQueue = "order-queue";
        public const string RegisterOrderCommand = "order.register.queue";
        public const string SendOrderToUserEmailCommand = "order.send.email.queue";
        public const string UpdateOrderStatusCommand = "order.update.status.queue";
    }
}