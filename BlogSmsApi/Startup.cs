using Autofac;
using Blog.Sms.Application.EventHandler.EmailEventHandler;
using Blog.Sms.Application.Service;
using Blog.Sms.Application.Service.Imp;
using Blog.Sms.Repository;
using Blog.Sms.Repository.DB;
using Blog.Sms.Repository.Imp;
using Core.Aop;
using Core.Common.Filter;
using Core.CPlatform;
using Core.EventBus;
using Core.Log;
using Core.Socket.Singalr;
using Core.Swagger;
using Core.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogSmsApi
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
            containerBuilder.Builder("Blog.Sms.Application");
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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
            services.AddEventBus();
            services.AddMvc(s => s.Filters.Add<GlobaExceptionFilterAttribute>());
            services.AddControllers();
            services.AddSwagger("SmsApi", "v1");
            services.AddDbContext<DBContext>(options =>
            {
                string connection = Configuration.GetConnectionString("MySqlConnection");
                options.UseMySQL(connection);
            });
            services.AddSingalrServices() ;
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISingalrService, SingalrService>();
            services.AddScoped<ISysConfigRepository, SysConfigRepository>();
            services.AddInterceptorServices();
            services.AddConsul();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<EmailData, CreateEmailHandler>();
            loggerFactory.AddLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("cors");

            app.UseSwagger("SmsApi");

            app.UseRouting();

            app.UseAuthorization();

            app.UseConsul();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SingalrClient>("/SingalrClient");
                endpoints.MapControllers();
            });
        }
    }
}
