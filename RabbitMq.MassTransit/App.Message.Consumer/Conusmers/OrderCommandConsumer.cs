using App.MassTransit.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace App.Message.Consumer.Conusmers
{
    public class OrderCommandConsumer : IConsumer<IOrderCommand>
    {
        public async Task Consume(ConsumeContext<IOrderCommand> context)
        {
            Console.WriteLine("consumer ....");
            Console.WriteLine("context", context.Message.CorrelationId +"titre : " + context.Message.Titre);
        }
    }
}
