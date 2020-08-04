using App.MassTransit.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace App.Demo.Web.Consumers
{
    public class OrderCommandConsumer : IConsumer<IOrderCommand>
    {
        private readonly ILogger<OrderCommandConsumer> logger;
        public OrderCommandConsumer(ILogger<OrderCommandConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderCommand> context)
        {
            logger?.LogInformation("Starting demo web ...");
            logger?.LogInformation("orderconsumer demo web",context.Message);
        }
    }
}
