using Core.CPlatform.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Sms.Domain
{
   public class SysConfig:Entity<int>
    {
        /// <summary>
        /// 配置的key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 配置的value
        /// </summary>
        public string Value { get; set; }
    }
}
