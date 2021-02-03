using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Aop.Polly
{
   public interface IPollyInterceptor
    {
        void Intercept(PollyAttribute attribute, IInvocation invocation);
    }
}
