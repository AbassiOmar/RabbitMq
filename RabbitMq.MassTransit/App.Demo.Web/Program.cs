using App.MassTransit.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace App.Demo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .AddRabitMqSettings()
            .ConfigureWebHostDefaults(webbuilder =>
            {
                webbuilder.UseStartup<Startup>();
            });
    }
}
