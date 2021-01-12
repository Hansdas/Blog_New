using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Log
{
   public class LogOption
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogProvider { get; set; }
        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool EnableLog { get; set; }
        /// <summary>
        /// 日志配置文件路径
        /// </summary>
        public string ConfigPath { get; set; }
    }
}
