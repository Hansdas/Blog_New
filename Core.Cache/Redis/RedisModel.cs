using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Cache.Redis
{
   public class RedisModel
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string InstanceName { get; set; }
        public int DefaultDB { get; set; }
        public string Password { get; set; }
    }
}
