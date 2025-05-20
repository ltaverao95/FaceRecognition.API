namespace Notification.Customer.NotificationService.Core.Consumers
{
    using MassTransit;
    using Messaging.Core.Constants;

    public class SendOrderToUserEmailConsumerDefinition : ConsumerDefinition<SendOrderToUserEmailConsumer>
    {
        public SendOrderToUserEmailConsumerDefinition()
        {
            EndpointName = RabbitMqMassTransitConstants.SendOrderToUserEmailCommand;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                                  IConsumerConfigurator<SendOrderToUserEmailConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(2, 1000));
        }
    }
}