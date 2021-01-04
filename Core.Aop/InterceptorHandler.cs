using Castle.DynamicProxy;
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
        public InterceptorHandler()
        {

        }
        public InterceptorHandler(ITransactionInterceptor transactionInterceptor)
        {
            _transactionInterceptor = transactionInterceptor;
        }
        public void Intercept(IInvocation invocation)
        {
            Attribute attribute = GetAttribute(invocation.MethodInvocationTarget ?? invocation.Method) as Attribute;
            if (attribute is TransactionAttribute)
                _transactionInterceptor.Intercept((TransactionAttribute)attribute, invocation);
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
