using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp.MVC.Messaging
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConnectionManager _mqManager;

        public Worker(ILogger<Worker> logger, IConnectionManager mqManager)
        {
            _logger = logger;
            _mqManager = mqManager;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            _mqManager.Connect();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Heartbeat @ {time}", DateTimeOffset.Now);
                await Task.Delay(30000, cancellationToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _mqManager.Disconnect();
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");
            await base.StopAsync(cancellationToken);
        }
    }
}
