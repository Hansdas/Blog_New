using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.CPlatform
{
   public class ConfigureProvider
    {
        public static IConfiguration configuration;
        private ConfigureProvider()
        {

        }
        public ConfigureProvider(params string[] paths)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            foreach(var path in paths)
            {
                builder.AddJsonFile(path, false, true);
            }
            //builder.AddJsonFile("Configs/appsettings.json", false, true);
            //builder.AddJsonFile("Configs/connection.json", false, true);
            configuration = builder.Build();
        }
        /// <summary>
        /// 将json配置项转为对应的model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T BuildModel<T>(string key) where T : class, new()
        {
            IServiceCollection descriptors = new ServiceCollection().AddOptions();
            IConfigurationSection section = configuration.GetSection(key);
            var model = descriptors.Configure<T>(section).BuildServiceProvider().GetService<IOptions<T>>().Value;
            return model;
        }
    }
}
