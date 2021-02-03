
using Core.CPlatform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Core.Auth.IdentityServer4
{
    public static class ConfigureIdentityServer4
    {
        public static IServiceCollection AddIdentityServer4(this IServiceCollection services)
        {
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            IIdentityServerBuilder identityServerBuilder = services.AddIdentityServer();
            //identityServerBuilder.AddDeveloperSigningCredential();
            identityServerBuilder.AddSigningCredential(new X509Certificate2(Path.Combine(basePath,
                ConfigureProvider.configuration["Certificate:Path"]),
                  ConfigureProvider.configuration["Certificate:Password"]));
            identityServerBuilder.AddInMemoryIdentityResources(ApiConfig.GetIdentityResources())
                .AddInMemoryApiResources(ApiConfig.GetApiResources())
                .AddInMemoryClients(ApiConfig.GetClients())
                .AddInMemoryApiScopes(ApiConfig.GetApiScopes());
            return services;
        }
    }
}
