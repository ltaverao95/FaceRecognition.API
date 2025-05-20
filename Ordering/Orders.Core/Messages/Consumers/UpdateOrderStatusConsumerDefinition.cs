namespace Orders.Core.Messages.Consumers
{
    using MassTransit;
    using Messaging.Core.Constants;

    public class UpdateOrderStatusConsumerDefinition : ConsumerDefinition<UpdateOrderStatusConsumer>
    {
        public UpdateOrderStatusConsumerDefinition()
        {
            EndpointName = RabbitMqMassTransitConstants.UpdateOrderStatusCommand;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                                  IConsumerConfigurator<UpdateOrderStatusConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(2, 1000));
        }
    }
}