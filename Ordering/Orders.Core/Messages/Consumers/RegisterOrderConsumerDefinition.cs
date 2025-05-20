namespace Orders.Core.Messages.Consumers
{
    using MassTransit;
    using Messaging.Core.Constants;

    public class RegisterOrderConsumerDefinition : ConsumerDefinition<RegisterOrderConsumer>
    {
        public RegisterOrderConsumerDefinition()
        {
            EndpointName = RabbitMqMassTransitConstants.RegisterOrderCommand;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                                  IConsumerConfigurator<RegisterOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(2, 1000));
        }
    }
}