using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Auth;
using Core.Auth.IdentityServer4;
using Core.CPlatform;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace BlogGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            string[] paths = { "appsettings.json" };
            new ConfigureProvider(paths);
            Configuration = ConfigureProvider.configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region IdentityServerAuthenticationOptions => need to refactor
            Action<IdentityServerAuthenticationOptions> webOption = option =>
            {
                option.Authority = Configuration["IdentityService:Uri"];
                option.ApiName = Configuration["IdentityService:ApiName:WebApi"];
                option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
            };
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
           .AddIdentityServerAuthentication("ApiAuthKey", webOption);
            #endregion
            services.AddOcelot(new ConfigurationBuilder()
            .AddJsonFile("Ocelot.json")
            .Build())
            .AddPolly()
            .AddConsul(); 
            services.AddCors(s =>
            {
                IConfigurationSection section = Configuration.GetSection("policy");
                string[] origins = section.GetSection("origins").Value.Split(',');
                string[] headers = section.GetSection("headers").Value.Split(",");
                s.AddPolicy("cors",
                    p => p.AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(origins)
                    .WithHeaders(headers)
                    );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("cors");

            app.UseRouting();

            await app.UseOcelot();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
