﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SampleApp.MVC.Messaging.Consumers;
using SampleApp.MVC.Messaging.Publishers;
using System;
using System.Threading;

namespace SampleApp.MVC.Messaging
{
    public class RabbitMqConnectionManager : IConnectionManager
    {
        private readonly ILogger<RabbitMqConnectionManager> _logger;
        private readonly BaseSettings _baseSettings;
        private readonly EnvironmentSettings _envSettings;
        private readonly IMessageHandler _messageHandler;
        
        private IPublisher _publisher;

        private IConnection _conn;
        private IModel _channel;

        public RabbitMqConnectionManager(ILogger<RabbitMqConnectionManager> logger, IOptions<BaseSettings> options, EnvironmentSettings envSettings, IMessageHandler messageHandler)
        {
            _logger = logger;
            _baseSettings = options.Value;
            _envSettings = envSettings;
            _messageHandler = messageHandler;
        }

        public void Connect()
        {
            var factory = new ConnectionFactory
            {
                UserName = _baseSettings.RabbitMqConnection.UserName,
                Password = _baseSettings.RabbitMqConnection.Password,
                VirtualHost = _baseSettings.RabbitMqConnection.VirtualHost,
                HostName = _baseSettings.RabbitMqConnection.HostName,
                AutomaticRecoveryEnabled = _baseSettings.RabbitMqConnection.AutomaticRecoveryEnabled,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(_baseSettings.RabbitMqConnection.NetworkRecoveryInterval)
            };

            var connected = false;
            var retryCount = 3;

            while(retryCount > 0 && !connected)
            {
                try
                {
                    _conn = factory.CreateConnection();
                    connected = true;
                }
                catch (BrokerUnreachableException)
                {
                    Thread.Sleep(5000);
                }
                finally
                {
                    retryCount--;
                }
            }

            if (!connected)
                throw new Exception("FAILED TO CONNECT TO RABBIT!");
            
            _channel = _conn.CreateModel();

            _channel.Initiate(_baseSettings);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ConsumeHandler;

            var consumerTag = _channel.BasicConsume($"{_baseSettings.MacroMarketIdentifierPrefix}-lobby-{_baseSettings.ServiceName.ToLower()}-inbound-queue", false, consumer);

            _publisher = new RabbitMqPublisher(_baseSettings, _channel);

            _logger.LogInformation($"Consumer Tag: {consumerTag}");

        }

        private void ConsumeHandler(object ch, BasicDeliverEventArgs ea)
        {
            _channel.BasicAck(ea.DeliveryTag, false);
            _messageHandler.Execute(ea, _publisher);
        }

        public void Disconnect()
        {
            _channel.Close();
            _conn.Close();
        }
    }
}
