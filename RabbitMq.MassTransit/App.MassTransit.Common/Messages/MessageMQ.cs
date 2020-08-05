using System;
namespace App.MassTransit.Common.Messages
{
    public interface IMessageMQ
    {
        Guid Id { get;  }
        string Message { get; }
    }
}
