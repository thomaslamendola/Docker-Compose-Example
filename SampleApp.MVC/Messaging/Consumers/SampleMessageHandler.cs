using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using SampleApp.MVC.Messaging.Publishers;

namespace SampleApp.MVC.Messaging.Consumers
{
    public class SampleMessageHandler : IMessageHandler
    {
        private readonly ILogger<SampleMessageHandler> _logger;

        public SampleMessageHandler(ILogger<SampleMessageHandler> logger)
        {
            _logger = logger;
        }

        public void Execute(BasicDeliverEventArgs ea, IPublisher publisher)
        {
            var body = ea.Body.ToArray();
            var model = Utf8Json.JsonSerializer.Deserialize<TestModel>(body);

            _logger.LogInformation(model.Test);

        }
    }

    public class TestModel
    {
        public string Test { get; set; }
    }

    public interface IMessageHandler
    {
        void Execute(BasicDeliverEventArgs ea, IPublisher publisher);
    }
}
