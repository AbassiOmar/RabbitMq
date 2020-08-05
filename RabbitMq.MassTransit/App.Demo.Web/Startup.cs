using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Demo.Web.Consumers;
using App.MassTransit.Common;
using App.MassTransit.Common.Consumers;
using App.MassTransit.Common.Messages;
using App.MassTransit.Messages;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace App.Demo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<RabbitMqSettings>(Configuration.GetSection("RabbitMq"));
            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq(ConfigureRabbitMq);
                cfg.AddConsumersFromNamespaceContaining<MessageConsumer>();
                cfg.AddConsumer<OrderCommandConsumer>();
            });
            services.AddHostedService<BusService>();
        }

        private void ConfigureRabbitMq(IBusRegistrationContext busRegistrationContext, IRabbitMqBusFactoryConfigurator configurationBus)
        {
            var rabbitMQConfigurationOption = busRegistrationContext.GetService<IOptions<RabbitMqSettings>>();
            var rabbitMQConfiguration = rabbitMQConfigurationOption.Value;

            configurationBus.Host(new Uri($"rabbitmq://{rabbitMQConfiguration.Host}/{rabbitMQConfiguration.VirtualHost}"), cfgRabbitMq =>
            {
                cfgRabbitMq.Username(rabbitMQConfiguration.Username);
                cfgRabbitMq.Password(rabbitMQConfiguration.Password);
            });

            configurationBus.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(IOrderCommand)),
                                cfgEndpoint =>
                                {
                                    cfgEndpoint.ConfigureConsumer<OrderCommandConsumer>(busRegistrationContext);
                                    cfgEndpoint.ConfigureConsumers(busRegistrationContext);
                                    cfgEndpoint.UseRetry(cfgRetry =>
                                    {
                                        cfgRetry.Interval(3, TimeSpan.FromSeconds(5));
                                    });
                                });

            //configurationBus.ReceiveEndpoint("IMessageMQ", cfgEndpoint =>
            //{
            //    cfgEndpoint.ConfigureConsumers(busRegistrationContext);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
