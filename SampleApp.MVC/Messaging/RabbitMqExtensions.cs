using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.MVC.Messaging
{
    public static class RabbitMqExtensions
    {
        public static IModel Initiate(this IModel _channel, BaseSettings settings)
        {
            var lobbyPrefix = $"{settings.MacroMarketIdentifierPrefix}-lobby";
            var servicePrefix = $"{lobbyPrefix}-{settings.ServiceName.ToLower()}";

            _channel.ExchangeDelete($"{servicePrefix}-inbound");

            _channel.ExchangeDeclare($"{servicePrefix}-inbound", ExchangeType.Topic, true);
            _channel.ExchangeDeclare($"{servicePrefix}-outbound", ExchangeType.Topic, true);

            foreach (var bindingKey in settings.Bindings)
            {
                _channel.ExchangeBind($"{servicePrefix}-inbound", $"{lobbyPrefix}-inbound", bindingKey);
            }

            _channel.ExchangeBind($"{servicePrefix}-outbound", $"{lobbyPrefix}-outbound", "#");

            _channel.QueueDeclare($"{lobbyPrefix}-inbound-queue", false, false, false, null);
            _channel.QueueBind($"{servicePrefix}-inbound-queue", $"{servicePrefix}-inbound", "#", null);

            return _channel;
        }
    }
}
