using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Socket.Singalr
{
   public static class SingalrServiceBuilder
    {
        public static IServiceCollection AddSingalrServices(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSignalR();
            serviceDescriptors.AddScoped<ISingalrContent, SingalrContent>();
            return serviceDescriptors;
        }
    }
}
