using App.Messaging.Configurations;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace App.Messaging
{
   public static class ConfigurationExtensions
    {

        public static IBusFactoryConfigurator ConfigureEndpoints(this IBusFactoryConfigurator configurator, IServiceProvider serviceProvider, IConfiguration configurationSection, params Type[] consumers)
        {
            var endPointDefinitions = configurationSection.Get<IEnumerable<EndpointDefinition>>();
            foreach (var endPointDefinition in endPointDefinitions)
            {
                configurator = ConfigureEndpoint(configurator, serviceProvider, endPointDefinition, consumers);
            }
            return configurator;
        }

        public static IBusFactoryConfigurator ConfigureEndpoint(IBusFactoryConfigurator configurator, IServiceProvider serviceProvider, EndpointDefinition endPointDefinition, params Type[] consumers)
        {
            configurator.ReceiveEndpoint(endPointDefinition.Name, cfgEndPoint =>
            {
                if (consumers != null && consumers.Length > 0)
                {
                    cfgEndPoint.ConfigureConsumer(serviceProvider, consumers);
                }
                else
                {
                    cfgEndPoint.ConfigureConsumers(serviceProvider);
                }

                if (endPointDefinition.RetryPolicy != null)
                {
                    cfgEndPoint.ConfigureRetry(endPointDefinition.RetryPolicy);
                }
            });
            return configurator;
        }

        public static IReceiveEndpointConfigurator ConfigureRetry(this IReceiveEndpointConfigurator cfgEndPoint, RetryPolicy retryPolicy)
        {
            cfgEndPoint.UseMessageRetry(cfgRetry =>
            {
                if (retryPolicy?.Interval != null)
                {
                    var intervalRetryPolicy = retryPolicy.Interval;
                    cfgRetry.Interval(intervalRetryPolicy.Count, intervalRetryPolicy.Value);
                }
                if (retryPolicy?.Immediate.HasValue ?? false)
                {
                    cfgRetry.Immediate(retryPolicy.Immediate.Value);
                }
                if (retryPolicy?.Intervals?.Length > 0)
                {
                    cfgRetry.Intervals(retryPolicy.Intervals);
                }
                if (retryPolicy?.Exponential != null)
                {
                    var exponentialInterval = retryPolicy.Exponential;
                    cfgRetry.Exponential(exponentialInterval.Count, exponentialInterval.MinInterval, exponentialInterval.MaxInterval, exponentialInterval.IntervalDelta);
                }
            });
            return cfgEndPoint;
        }

        public static IRabbitMqBusFactoryConfigurator ConfigureRabbitMQConnection(this IRabbitMqBusFactoryConfigurator configurator, RabbitMQConnectionSettings connectionSettings)
        {
            configurator.Host(new Uri($"rabbitmq://{connectionSettings.HostName}/{connectionSettings.VirtualHost}"), connectionSettings.ApplicationName, cfgHost =>
            {
                cfgHost.Username(connectionSettings.User);
                cfgHost.Password(connectionSettings.Password);
            });
            return configurator;
        }
    }
}
