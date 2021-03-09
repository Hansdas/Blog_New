using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Quartz.Application.Service;
using Blog.Quartz.Application.Service.Imp;
using Blog.Quartz.Repository;
using Blog.Quartz.Repository.DB;
using Blog.Quartz.Repository.Imp;
using Core.Common.Filter;
using Core.CPlatform;
using Core.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace Blog.Quartz.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            string[] paths = { "Configs/appsettings.json", "Configs/connection.json" };
            new ConfigureProvider(paths);
            Configuration = ConfigureProvider.configuration;
        }

        public IConfiguration Configuration { get; }
        IServiceCollection serviceDescriptors;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddScoped<IQuartzOptionRepository,QuartzOptionRepository>();
            services.AddScoped<IQuartzOptionService, QuartzOptionService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<DBContext>(options => {
                string connection = Configuration.GetConnectionString("MySqlConnection");
                options.UseMySQL(connection);
            });
            services.AddMvc(s => s.Filters.Add<GlobaExceptionFilterAttribute>());
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHttpClient();
            serviceDescriptors = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            ServiceLocator.Init(serviceDescriptors, s => s.BuildServiceProvider());
            IQuartzOptionService quartzOptionService = ServiceLocator.Get<IQuartzOptionService>();
            quartzOptionService.Init();
            loggerFactory.AddLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
