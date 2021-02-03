using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Core.Aop.Polly;
using Core.Aop.Transaction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core.Aop
{
    public static class AspectBuilder
    {
        /// <summary>
        /// 注册aop
        /// </summary>
        /// <param name="assemblyName"></param>
        public static void Builder(this ContainerBuilder containerBuilder, string assemblyName)
        {
            containerBuilder.Builder<IInterceptorTag, InterceptorHandler>(assemblyName);

        }
        /// <summary>
        /// 注册aop
        /// </summary>
        /// <typeparam name="IInterceptor">拦截器标记</typeparam>
        /// <typeparam name="TInterceptorHandler">拦截器处理</typeparam>
        /// <param name="containerBuilder"></param>
        /// <param name="assemblyName"></param>
        public static void Builder<ImpInterceptor, TInterceptorHandler>(this ContainerBuilder containerBuilder, string assemblyName)
            where TInterceptorHandler : IInterceptor
            where ImpInterceptor : class
        {
            Assembly assembly = Assembly.Load(assemblyName);
            containerBuilder.RegisterType<TInterceptorHandler>();
            containerBuilder.RegisterAssemblyTypes(assembly).Where(type => typeof(ImpInterceptor).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope()
                  .EnableInterfaceInterceptors()
                  .InterceptedBy(typeof(TInterceptorHandler)); 

        }
        public static IServiceCollection AddInterceptorServices(this IServiceCollection services)
        {
            services.AddScoped<ITransactionInterceptor, TransactionInterceptor>();
            services.AddScoped<IPollyInterceptor, PollyInterceptor>();
            return services;
        }
    }
}
