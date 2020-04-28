using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utf8Json.Resolvers;

namespace SampleApp.MVC.Messaging.Publishers
{
    public class RabbitMqPublisher : IPublisher
    {
        private readonly IModel _channel;
        private readonly BaseSettings _baseSettings;

        public RabbitMqPublisher(BaseSettings baseSettings, IModel channel)
        {
            _channel = channel;
            _baseSettings = baseSettings;
        }

        public Task Execute(object message, string routingKey, IDictionary<string, object> headers = null)
        {
            return Task.Run(() =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.Headers = headers;
                var body = Utf8Json.JsonSerializer.Serialize(message, StandardResolver.ExcludeNullCamelCase);
                _channel.BasicPublish($"{_baseSettings.MacroMarketIdentifierPrefix}-lobby-{_baseSettings.ServiceName.ToLower()}-outbound", 
                    routingKey, properties, body);
            });
        }
    }
}
