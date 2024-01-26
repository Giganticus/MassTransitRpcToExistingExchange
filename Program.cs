using System;
using System.Threading.Tasks;
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
                           
                            cfg.UseRawJsonSerializer(isDefault:true);
                            cfg.UseNewtonsoftRawJsonSerializer();
                            cfg.UseRawJsonDeserializer(isDefault:true);
                            cfg.UseNewtonsoftRawJsonDeserializer();
                        });
                        x.AddRequestClient<JObject>(new Uri("exchange:my.existing.exchange?type=direct"));
                    });
                   
                   services.AddHostedService<WorkflowRequester>();
                });
    }
}