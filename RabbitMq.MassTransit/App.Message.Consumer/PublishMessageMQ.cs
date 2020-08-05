using App.MassTransit.Common.Messages;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Message.Consumer
{
    public class PublishMessageMQ:BackgroundService
    {
        private readonly IPublishEndpoint publishProvider;

        public PublishMessageMQ(IPublishEndpoint publishProvider)
        {
            this.publishProvider = publishProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.publishProvider.Publish<IMessageMQ>(new { Id = new Guid(), Message = "publish from service consumer" });
        }
    }
}
