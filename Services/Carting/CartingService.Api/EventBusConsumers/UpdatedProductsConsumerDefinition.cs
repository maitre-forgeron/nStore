using MassTransit;

namespace CartingService.Api.EventBusConsumers
{
    public class UpdatedProductsConsumerDefinition : ConsumerDefinition<UpdatedProductsConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UpdatedProductsConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
            endpointConfigurator.UseMessageRetry(r => r.Immediate(5));
        }
    }
}
