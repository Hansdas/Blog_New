using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Core.Auth.IdentityServer4
{
    public static class ConfigureIdentityServer4
    {
        public static IServiceCollection AddIdentityServer4(this IServiceCollection services)
        {
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            IIdentityServerBuilder identityServerBuilder = services.AddIdentityServer();
            identityServerBuilder.AddDeveloperSigningCredential();           
            identityServerBuilder.AddInMemoryIdentityResources(ApiConfig.GetIdentityResources())
                .AddInMemoryApiResources(ApiConfig.GetApiResources())
                .AddInMemoryClients(ApiConfig.GetClients())
                .AddInMemoryApiScopes(ApiConfig.GetApiScopes());
            return services;
        }
    }
}
