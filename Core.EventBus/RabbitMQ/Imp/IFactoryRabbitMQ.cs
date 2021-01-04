using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventBus.RabbitMQ.Imp
{
    public interface IFactoryRabbitMQ
    {
        IConnection CreateConnection();
    }
}
