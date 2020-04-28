using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.MVC.Messaging
{
    public class BaseSettings
    {
        public string ServiceName { get; set; }
        public string MacroMarketIdentifierPrefix { get; set; }
        public List<string> Bindings { get; set; }
        public RabbitMqConnection RabbitMqConnection {get;set;}
}

    public class RabbitMqConnection
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public ushort NetworkRecoveryInterval { get; set; }
    }

    public class EnvironmentSettings
    {
        public EnvironmentSettings()
        {
            Rabbit = new RabbitMqConnection();
        }
        public RabbitMqConnection Rabbit { get; set; }
    }
}
