using App.MassTransit.Messages;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Message.ProductOrder
{
    public class SendOrderService : BackgroundService
    {
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly ILogger<SendOrderService> logger;
        public SendOrderService(ISendEndpointProvider sendEndpointProvider, ILogger<SendOrderService> logger)
        {
            this.sendEndpointProvider = sendEndpointProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var uri  = new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(nameof(IOrderCommand))}");
        var sendEndpoint = await this.sendEndpointProvider.GetSendEndpoint(uri);
            await sendEndpoint.Send<IOrderCommand>(new { Numero = "1", Titre = "title01" });
            Console.WriteLine("order sended");
        }
    }
}
