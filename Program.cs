using System;
using System.Threading.Tasks;
using GettingStarted.Contracts;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });
                           
                            //cfg.ConfigureEndpoints(context);
                            
                            cfg.ConfigureSend(x =>
                            {
                                x.UseSendExecute(c =>
                                {
                                    c.Headers.Set("FLOW", "FlowName");
                                });
                            });
                        });
                        
                        x.AddRequestClient<WorkflowRequest>(new Uri("exchange:my.existing.exchange?type=direct"));
                    });
                   
                   services.AddHostedService<WorkflowRequester>();
                });
    }
}
