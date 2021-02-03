using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CPlatform
{
   public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
        public static T Get<T>() where T:class
        {
            T t = Instance.GetService(typeof(T)) as T;
            return t;
        }
    }
}
