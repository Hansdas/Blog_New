using Core.Configuration;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static IdentityServer4.IdentityServerConstants;

namespace Core.Auth.IdentityServer4
{
    public class ApiConfig
    {
        public static IConfiguration Configuration = ConfigureProvider.configuration;
        private static string[] ApiNames = { "WebApi" };
        /// <summary>
        /// Define which APIs will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> apiResources = new List<ApiResource>();
            foreach (var item in ApiNames)
            {
                var configuration = Configuration.GetSection(item);
                string name = configuration.GetSection("ApiResource").Value;
                string[] scopes = configuration.GetSection("ApiScope").Value.Split(",");
                ApiResource apiResource = new ApiResource(name, name) {
                    Scopes = scopes
                };
                apiResources.Add(apiResource);
            }
            return apiResources;
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            List<ApiScope> apiScopes = new List<ApiScope>();
            foreach (var item in ApiNames)
            {
                var configuration = Configuration.GetSection(item);
                string[] names = configuration.GetSection("ApiScope").Value.Split(",");
                foreach(var name in names)
                {
                    ApiScope apiScope = new ApiScope(name, name);
                    apiScopes.Add(apiScope);
                }
            }
            return apiScopes;
        }
        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            foreach(var item in ApiNames)
            {
                var configuration = Configuration.GetSection(item);
                string clientId = configuration["Client:ClientId"];
                string clientName = configuration["Client:ClientName"];
                Secret[] secrets = configuration["Client:ClientSecrets"].Split(',').Select(s=>new Secret(s.Sha256())).ToArray() ;
                string[] grantTypes = configuration["Client:AllowedGrantTypes"].Split(",");
                List<string> allowedScopes = configuration["Client:AllowedScopes"].Split(',').ToList();
                //allowedScopes.Add(IdentityServerConstants.StandardScopes.OpenId);
                //allowedScopes.Add(IdentityServerConstants.StandardScopes.Profile);
                allowedScopes.Add(StandardScopes.OfflineAccess);
                int accseeTokenTime =Convert.ToInt32(configuration["Client:AccseeTokenTime"]);
                Client client = new Client();
                client.ClientId = clientId;
                client.ClientName = clientName;
                client.ClientSecrets = secrets;
                client.AllowedGrantTypes = grantTypes;
                client.AllowedScopes = allowedScopes;
                client.AccessTokenLifetime = accseeTokenTime;
                client.AllowOfflineAccess = true;
                clients.Add(client);

            }
            return clients;
        }

        /// <summary>
        /// Define which IdentityResources will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
            };
        }
    }
}
