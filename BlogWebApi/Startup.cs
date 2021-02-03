using Autofac;
using Blog.Repository.DB;
using Core.Aop;
using Core.Common.Filter;
using Core.Consul;
using Core.CPlatform;
using Core.EventBus;
using Core.Log;
using Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogWebApi
{
    public class Startup
    {
        public Startup()
        {
            string[] paths = { "Configs/appsettings.json", "Configs/connection.json" };
            new ConfigureProvider(paths);
            Configuration = ConfigureProvider.configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.Builder<IInterceptorTag, InterceptorHandler>("Blog.Application");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwagger("WebApi","v1");
            services.AddDbContext<DBContext>(options => {
                string connection = Configuration.GetConnectionString("MySqlConnection");
                options.UseMySQL(connection);
            });
            services.AddApplicationService()
                .AddRepository()
                .AddCommon()
                .AddConsul()
                .AddInterceptorServices()
                .AddEventBus(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            ServiceLocator.Instance = app.ApplicationServices;
            loggerFactory.AddLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger("WebApi");

            app.UseRouting();

            app.UseConsul();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
