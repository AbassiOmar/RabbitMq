using App.MassTransit.Common.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.MassTransit.Common.Consumers
{
    public class MessageConsumer : IConsumer<IMessageMQ>
    {
        public async Task Consume(ConsumeContext<IMessageMQ> context)
        {
            Console.WriteLine("public messsage");
            Console.WriteLine(context.Message.Id + " - " + context.Message.Message);
        }
    }
}
