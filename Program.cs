using System;
using System.Threading.Tasks;
using GettingStartedClient.Contracts;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace GettingStartedClient
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
                            
                            //cfg.UseNewtonsoftJsonSerializer();
                            cfg.UseRawJsonDeserializer();
                            cfg.UseNewtonsoftRawJsonDeserializer();
                            cfg.UseNewtonsoftRawJsonSerializer();
                            
                        });
                        
                                                
                        x.AddRequestClient<JObject>(new Uri("exchange:my.existing.exchange?type=direct"));
                    });
                   
                   services.AddHostedService<WorkflowRequester>();
                });
    }
}