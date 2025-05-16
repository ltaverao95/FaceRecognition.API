namespace Messaging.EventBus.Services
{
    using MassTransit;
    using Microsoft.Extensions.Hosting;
    using System.Threading;
    using System.Threading.Tasks;

    public class BusService(IBus bus) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }
    }

    //public class BusService : IHostedService
    //{
    //    private readonly IBusControl busControl;

    //    public BusService(IBusControl busControl)
    //    {
    //        this.busControl = busControl;
    //    }

    //    public Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        return this.busControl.StartAsync(cancellationToken);
    //    }

    //    public Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        return this.busControl.StopAsync(cancellationToken);
    //    }
    //}
}