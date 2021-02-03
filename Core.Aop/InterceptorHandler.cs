using Castle.DynamicProxy;
using Core.Aop.Polly;
using Core.Aop.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Aop
{
    public class InterceptorHandler : IInterceptor
    {
        private readonly ITransactionInterceptor _transactionInterceptor;
        private readonly IPollyInterceptor _pollyInterceptor;
        public InterceptorHandler()
        {

        }
        public InterceptorHandler(ITransactionInterceptor transactionInterceptor, IPollyInterceptor pollyInterceptor)
        {
            _transactionInterceptor = transactionInterceptor;
            _pollyInterceptor = pollyInterceptor;
        }
        public void Intercept(IInvocation invocation)
        {
            Attribute attribute = GetAttribute(invocation.MethodInvocationTarget ?? invocation.Method) as Attribute;
            if (attribute is TransactionAttribute)
                _transactionInterceptor.Intercept((TransactionAttribute)attribute, invocation);
            else if (attribute is PollyAttribute)
                _pollyInterceptor.Intercept((PollyAttribute)attribute, invocation);
            else
                invocation.Proceed();
        }
        private object GetAttribute(MethodInfo method)
        {
            object[] attributes= method.GetCustomAttributes(true);
            return attributes.FirstOrDefault();
        }
    }
}
