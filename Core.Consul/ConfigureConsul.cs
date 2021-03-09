using Consul;
using Core.CPlatform;
using Core.Log;
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
        public static IApplicationBuilder UseConsul(this IApplicationBuilder builder)
        {
            ConsulOption model = ConfigureProvider.BuildModel<ConsulOption>("Consul");
            if (!model.Enable)
                return builder;
            IConsulClient client = builder.ApplicationServices.GetRequiredService<IConsulClient>();
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
            try
            {
                client.Agent.ServiceRegister(registration).Wait();
                var lifeTime = builder.ApplicationServices.GetRequiredService<IApplicationLifetime>();
                lifeTime.ApplicationStopping.Register(() => {
                    client.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, "Core.Consul", ex.Message);
            }

            return builder;
        }
    }
}
