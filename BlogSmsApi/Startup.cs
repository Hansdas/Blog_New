using Blog.Sms.Application.EventHandler.EmailEventHandler;
using Blog.Sms.Application.Service;
using Blog.Sms.Application.Service.Imp;
using Blog.Sms.Repository;
using Blog.Sms.Repository.DB;
using Blog.Sms.Repository.Imp;
using Core.Common.Filter;
using Core.Configuration;
using Core.EventBus;
using Core.Socket.Singalr;
using Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogSmsApi
{
    public class Startup
    {
        public Startup()
        {
            string[] paths = { "Configs/appsettings.json"};
            new ConfigureProvider(paths);
            Configuration = ConfigureProvider.configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(s =>
            {
                s.AddPolicy("cores", build =>
                {
                    IConfigurationSection section = Configuration.GetSection("Policy");
                    string[] origins = section.GetSection("Origins").Value.Split(',');
                    string[] headers = section.GetSection("Headers").Value.Split(',');
                    build.WithOrigins(origins)
                    .WithHeaders(headers)
                    .AllowAnyMethod()
                    .AllowCredentials();

                });

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
            services.AddScoped<ISysConfigRepository, SysConfigRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<EmailData, CreateEmailHandler>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("cores");

            app.UseSwagger("SmsApi");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SingalrClient>("/chatHub");
                endpoints.MapControllers();
            });
        }
    }
}
