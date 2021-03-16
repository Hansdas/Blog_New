using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
  public  class EventBusException: Exception
    {
        public EventBusException()
        {

        }
        public EventBusException(string message) : base(message)
        {

        }
        public EventBusException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
