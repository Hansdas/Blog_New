using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Swagger
{
   public static class ConfigureSwagger
    {
        public static void AddSwagger(this IServiceCollection services,string title,string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }
        public static void UseSwagger(this IApplicationBuilder app,string name)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", name);
            });
        }
    }
}
