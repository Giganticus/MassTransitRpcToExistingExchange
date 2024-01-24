using MassTransit;

namespace GettingStarted.Contracts;


// [ExcludeFromTopology]
//[EntityName("workflow.trigger.flow")]
[EntityName("my.existing.exchange")]
public class WorkflowRequest
{
    public string Payload { get; init; }
}