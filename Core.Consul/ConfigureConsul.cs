using Consul;
using Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Consul
{
  public static  class ConfigureConsul
    {
        public static IServiceCollection AddConsul(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors.AddSingleton<IConsulClient, ConsulClient>(s => new ConsulClient(p => {
                p.Address = new Uri(ConfigureProvider.configuration.GetSection("ConsulService").Value);
                p.Datacenter = "dc1";
            }
            ));
        }
        public static IApplicationBuilder UserConsul(this IApplicationBuilder builder)
        {
            IConsulClient client = builder.ApplicationServices.GetRequiredService<IConsulClient>();

            ConsulOption model = ConfigureProvider.BuildModel<ConsulOption>("Consul");
            string http = string.Format("{0}://{1}:{2}/api/health", model.Schem, model.Host, model.Port);

            AgentServiceCheck httpCheck = new AgentServiceCheck();
            httpCheck.HTTP = http;
            httpCheck.Interval = TimeSpan.FromSeconds(model.Interval);
            httpCheck.DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(model.RemoveAfterError);
            AgentServiceRegistration registration = new AgentServiceRegistration();
            registration.Address = model.Host;
            registration.Port = Convert.ToInt32(model.Port);
            registration.ID = string.Format("{0}.{1}", model.Host, model.Port);
            registration.Name = model.Name;
            registration.Check = httpCheck;

            client.Agent.ServiceRegister(registration).Wait();

            var lifeTime = builder.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            lifeTime.ApplicationStopping.Register(() => {
                client.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return builder;
        }
    }
}
