using Core.EventBus.RabbitMQ.Imp;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        private string queueName = "";
        /// <summary>
        /// 交换机名称
        /// </summary>
        private string exchangeName = "exchange-name";
        /// <summary>
        /// 交换类型
        /// </summary>
        private string exchangeType = "direct";
        private IFactoryRabbitMQ _factory;
        private IEventBusManager _eventBusManager;
        private ILogger<EventBusRabbitMQ> _log;
        private readonly IConnection connection;
        private readonly IModel channel;

        public EventBusRabbitMQ(IFactoryRabbitMQ factory, IEventBusManager eventBusManager, ILogger<EventBusRabbitMQ> log)
        {
            _factory = factory;
            _eventBusManager = eventBusManager;
            _eventBusManager.OnRemoveEventHandler += OnRemoveEvent;
            _log = log;
            connection = _factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, exchangeType);
            queueName = InitializeEventConsumer(queueName);
        }
        private void OnRemoveEvent(object sender, ValueTuple<Type, Type> args)
        {
            channel.QueueUnbind(queueName, exchangeName, args.Item1.Name);
        }
        public void Publish(EventData eventData)
        {
            string routeKey = eventData.GetType().Name;
            string message = JsonConvert.SerializeObject(eventData);
            byte[] body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchangeName, routeKey, null, body);
        }

        public void Subscribe<T, TH>()
            where T : EventData
            where TH : IEventHandler
        {
            _eventBusManager.AddSub<T, TH>();
            channel.QueueBind(queueName,exchangeName, typeof(T).Name);
         
        }
        private string InitializeEventConsumer(string queue)
        {
            var localQueueName = queue;
            if (string.IsNullOrEmpty(localQueueName))
            {
                localQueueName = channel.QueueDeclare().QueueName;
            }
            else
            {
                channel.QueueDeclare(localQueueName, true, false, false, null);
            }

            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += async (model, ea) =>
            {
                string eventName = ea.RoutingKey;
                byte[] resp = ea.Body.ToArray();
                string body = Encoding.UTF8.GetString(resp);
                _log.LogInformation(body);
                try
                {
                    Type eventType = _eventBusManager.FindEventType(eventName);
                    var eventData = (EventData)JsonConvert.DeserializeObject(body, eventType);
                    IEventHandler eventHandler = _eventBusManager.FindHandlerType(eventType) as IEventHandler;
                    await eventHandler.Handler(eventData);
                }
                catch (Exception ex)
                {
                    _log.LogInformation(ex.Message);
                    //throw ex;
                }
            };

            channel.BasicConsume(localQueueName, autoAck: true, consumer: consumer);

            return localQueueName;
        }
        public void Unsubscribe<T, TH>()
           where T : EventData
           where TH : IEventHandler
        {
            if (_eventBusManager.HaveAddHandler(typeof(T)))
            {
                _eventBusManager.RemoveEventSub<T, TH>();
            }
        }
    }
}
