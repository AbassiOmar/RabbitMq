using App.MassTransit.Common;
using App.MassTransit.Messages;
using App.Message.Consumer.Conusmers;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace App.Message.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .AddRabitMqSettings()
               .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
        {
            services.Configure<RabbitMqSettings>(hostingContext.Configuration.GetSection("RabbitMQ"));
            services.AddMassTransit(cfgGlobal =>
            {
                cfgGlobal.AddConsumer<OrderCommandConsumer>();
                cfgGlobal.UsingRabbitMq(ConfigureRabbitMq);
            });
            services.AddHostedService<BusService>();
        }

        private static void ConfigureRabbitMq(IBusRegistrationContext busRegistrationContext, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            var rabbitMQConfigurationOption = busRegistrationContext.GetService<IOptions<RabbitMqSettings>>();
            var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

            rabbitMqBusFactoryConfigurator.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
            {
                cfgRabbitMq.Username(rabbitMQConfiguration.Username);
                cfgRabbitMq.Password(rabbitMQConfiguration.Password);
            });
            rabbitMqBusFactoryConfigurator.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(IOrderCommand)),
                              cfgEndpoint =>
                              {
                                  cfgEndpoint.ConfigureConsumer<OrderCommandConsumer>(busRegistrationContext);
                                  cfgEndpoint.UseRetry(cfgRetry =>
                                  {
                                      cfgRetry.Interval(3, TimeSpan.FromSeconds(5));
                                  });
                              });
        }
    }
}
