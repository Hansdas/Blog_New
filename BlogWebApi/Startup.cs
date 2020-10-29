using Blog.Repository.DB;
using Core.Common.Filter;
using Core.Configuration;
using Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(s => s.Filters.Add<GlobaExceptionFilterAttribute>());
            services.AddControllers();
            services.AddSwagger("WebApi","v1");
            services.AddDbContext<DBContext>(options => {
                string connection = Configuration.GetConnectionString("MySqlConnection");
                options.UseMySQL(connection);
            });
            services.AddApplicationService().AddRepository().AddCommon();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger("WebApi");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
