using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace App.Messaging
{
    public class BusConfigurator: IHostedService
    {
            private readonly IBusControl _busControl;

            public BusConfigurator(IBusControl busControl)
            {
                _busControl = busControl;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                return _busControl.StartAsync(cancellationToken);
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return _busControl.StopAsync(cancellationToken);
            }
    }
}
