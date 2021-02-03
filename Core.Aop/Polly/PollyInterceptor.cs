using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Timeout;
using Polly.Wrap;

namespace Core.Aop.Polly
{
    public class PollyInterceptor : IPollyInterceptor
    {
        private ILogger<PollyInterceptor> _log;
        private readonly object _lock = new object();
        private static ConcurrentDictionary<MethodInfo, AsyncPolicyWrap> policies = new ConcurrentDictionary<MethodInfo, AsyncPolicyWrap>();
        public PollyInterceptor(ILogger<PollyInterceptor> log)
        {
            _log = log;
        }
        public void Intercept(PollyAttribute attribute, IInvocation invocation)
        {
            int? days = null;
            int retryTimes = attribute.RetryTimes;
            int retryIntervalMilliseconds = attribute.RetryIntervalMilliseconds;
            int exceptionsAllowedBeforeBreaking = attribute.ExceptionsAllowedBeforeBreaking;
            int millisecondsOfBreak = attribute.MillisecondsOfBreak;
            int timeOutMilliseconds = attribute.TimeOutMilliseconds;
            policies.TryGetValue(invocation.Method, out AsyncPolicyWrap policy);
            lock (policies)
            {
                if (policy == null)
                {
                    _log.LogInformation("开始");
                    //PolicyBuilder builder = Policy.Handle<Exception>();
                    var noOpPolicy = Policy.NoOpAsync();
                    //断路保护
                    policy = noOpPolicy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(millisecondsOfBreak), (ex, ts) =>
                    {
                        _log.LogInformation("断路保护中..."+ex.Message);
                    }, null));
                    //0表示不检测超时
                    if (timeOutMilliseconds > 0)
                        policy = policy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(timeOutMilliseconds), TimeoutStrategy.Pessimistic));
                    if (retryTimes > 0)
                        policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(retryTimes, i => TimeSpan.FromMilliseconds(retryIntervalMilliseconds), (ex, te) =>
                        {
                            _log.LogInformation("开始重试...");
                        }));
                    policies.TryAdd(invocation.Method, policy);
                    try
                    {
                        policy.ExecuteAsync(async () =>
                        {
                            _log.LogInformation("start job");
                            invocation.Proceed();
                            var task = (Task<bool>)invocation.ReturnValue;
                            return await task;
                        });
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation(ex.Message);
                    }

                }



            }

        }
    }
}
