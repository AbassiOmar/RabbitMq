using MassTransit;
using System;

namespace App.MassTransit.Messages
{
    public interface IOrderCommand : CorrelatedBy<Guid>
    {
        public string Numero { get; set; }

        public string Titre { get; set; }
    }
}
