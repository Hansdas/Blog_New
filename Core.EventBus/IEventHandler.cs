using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventBus
{
    public interface IEventHandler
    {
        Task Handler(EventData eventData);
    }
    public interface IEventHandler<T> : IEventHandler where T : EventData
    {
        //Task Handler(T eventData);
    }
}
