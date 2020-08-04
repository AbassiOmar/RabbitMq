using App.MassTransit.Common;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace App.Message.ProductOrder
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .AddRabitMqSettings()
                 .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext hostingContext,IServiceCollection services)
        {
            services.Configure<RabbitMqSettings>(hostingContext.Configuration.GetSection("RabbitMQ"));
            services.AddMassTransit(cfgGlobal =>
            {
                cfgGlobal.UsingRabbitMq(ConfigureRabbitMq);
            });
            services.AddHostedService<BusService>();
            services.AddHostedService<SendOrderService>();
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

        }
    }
}
