using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.MVC.Messaging.Publishers
{
    public interface IPublisher
    {
        Task Execute(object message, string routingKey, IDictionary<string, object> headers = null);
    }
}
