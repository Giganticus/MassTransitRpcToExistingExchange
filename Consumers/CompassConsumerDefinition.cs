// using MassTransit;
// using MassTransit.Configuration;
//
// namespace GettingStarted.Consumers;
//
// public class CompassConsumerDefinition : ConsumerDefinition<CompassConsumer>
// {
//     public CompassConsumerDefinition()
//     {
//         EndpointName = "workflow.trigger.flow";
//     }
//     
//     protected override void ConfigureConsumer(
//         IReceiveEndpointConfigurator endpointConfigurator, 
//         IConsumerConfigurator<CompassConsumer> consumerConfigurator,
//         IRegistrationContext context)
//     {
//         endpointConfigurator.ConfigureConsumeTopology = false;
//         endpointConfigurator.UseRawJsonDeserializer(isDefault:true);
//         //
//         //
//         // if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbit)
//         // {
//         //     rabbit.Bind("workflow.trigger.flow");
//         //     rabbit.BindQueue = true;
//         //         
//         // }
//     }
// }