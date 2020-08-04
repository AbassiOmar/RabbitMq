using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App.MassTransit.Common
{
    public static class ConfigurationExtensions
    {
        public static IHostBuilder AddRabitMqSettings(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration(config => config.AddJsonFile("rabbitmqsettings.json", optional: true, reloadOnChange: true));
        }
    }
}
