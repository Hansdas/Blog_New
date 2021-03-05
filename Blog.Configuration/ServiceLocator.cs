using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CPlatform
{
   public class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
        private static Func<IServiceCollection, IServiceProvider> _buildServiceProvider;
        private static IServiceCollection _ServiceDescriptors;
        public static void Init(IServiceCollection serviceDescriptors, Func<IServiceCollection, IServiceProvider> buildServiceProvider)
        {
            _ServiceDescriptors = serviceDescriptors;
            _buildServiceProvider = buildServiceProvider;
        }
        public static T Get<T>() where T:class
        {
            return _buildServiceProvider(_ServiceDescriptors).GetService<T>();
        }
    }
}
