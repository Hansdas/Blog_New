using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Auth;
using Core.Configuration;
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

namespace BlogGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            string[] paths = { "appsettings.json"};
            new ConfigureProvider(paths);
            Configuration = ConfigureProvider.configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOcelot(new ConfigurationBuilder()
            .AddJsonFile("Ocelot.json")
            .Build());
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
            TokenValidationParameters tokenValidationParameters = Jwt.GetTokenValidation();
            services.AddAuthentication()
             .AddJwtBearer("ApiAuthKey", x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.TokenValidationParameters = tokenValidationParameters;                
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("cors");

            app.UseRouting();

            await app.UseOcelot();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
