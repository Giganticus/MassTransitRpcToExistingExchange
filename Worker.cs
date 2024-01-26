using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GettingStartedClient.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GettingStartedClient;

public class WorkflowRequester(
    IHost host,
    ILogger<WorkflowRequester> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            
            var requestClient = (IRequestClient<JObject>)scope.ServiceProvider.GetService(typeof(IRequestClient<JObject>));
            
            if (requestClient == null)
                throw new Exception("Unable to resolve client");

            var jObject = JObject.Parse("{\"someJsonValueName\" : \"someJsonValue\"}");
            
            var response = await requestClient.GetResponse<JObject>(
                jObject,
                x => x.UseExecute(context => context.Headers.Set("FLOW", "RequestedFlowName")),
                stoppingToken, 
                timeout: RequestTimeout.None);

            logger.LogInformation($"Received response {JsonSerializer.Serialize(response)}");        
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}