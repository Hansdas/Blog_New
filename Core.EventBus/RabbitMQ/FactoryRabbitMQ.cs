using Core.EventBus.RabbitMQ.Imp;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventBus.RabbitMQ
{
    public class FactoryRabbitMQ : IFactoryRabbitMQ
    {
        private readonly IConnectionFactory connectionFactory;
        public FactoryRabbitMQ(EventBusOption eventBusOption)
        {
            IConnectionFactory conFactory = new ConnectionFactory
            {
                HostName = eventBusOption.Host,
                Port = eventBusOption.Port,
                UserName = eventBusOption.Username,
                Password = eventBusOption.Password,
                VirtualHost = "/"
            };
            connectionFactory = conFactory;
        }
        public IConnection CreateConnection()
        {
            return connectionFactory.CreateConnection();
        }
    }
}
