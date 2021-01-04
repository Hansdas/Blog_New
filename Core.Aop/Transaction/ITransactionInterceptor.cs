using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Aop.Transaction
{
   public interface ITransactionInterceptor
    {
        void Intercept(TransactionAttribute attribute, IInvocation invocation);
    }
}
