using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Aop.Polly
{
    /// <summary>
    /// 标记一个方法进行熔断
    /// </summary>
   public  class PollyAttribute: Attribute
    {
        public PollyAttribute(int retryTimes=3,int retryIntervalMilliseconds=100,int exceptionsAllowedBeforeBreaking=3
            ,int millisecondsOfBreak=3000,int timeOutMilliseconds=100000)
        {
            RetryTimes = retryTimes;
            RetryIntervalMilliseconds = retryIntervalMilliseconds;
            ExceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            MillisecondsOfBreak = millisecondsOfBreak;
            TimeOutMilliseconds = timeOutMilliseconds;

        }
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryTimes { get; set; } 
        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; }
        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; }
        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; }
        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; }

    }
}
