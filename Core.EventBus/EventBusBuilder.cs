using Core.Configuration;
using Core.EventBus.RabbitMQ;
using Core.EventBus.RabbitMQ.Imp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.EventBus
{
    public static class EventBusBuilder
    {
        public static EventBusOption eventBusOption;
        public static IServiceCollection AddEventBus(this IServiceCollection serviceDescriptors)
        {
            eventBusOption = ConfigureProvider.BuildModel<EventBusOption>("EventBusOption");
            switch (eventBusOption.MQProvider)
            {
                case MQProvider.RabbitMQ:
                    serviceDescriptors.AddSingleton<IEventBus, EventBusRabbitMQ>();
                    serviceDescriptors.AddSingleton(typeof(IFactoryRabbitMQ), factiory => {
                        return new FactoryRabbitMQ(eventBusOption);
                    });
                    break;
            }
            EventBusManager eventBusManager = new EventBusManager(serviceDescriptors, s => s.BuildServiceProvider());
            serviceDescriptors.AddSingleton<IEventBusManager>(eventBusManager);
            return serviceDescriptors;
        }
    }
}
