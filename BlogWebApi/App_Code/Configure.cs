using Blog.Application.Service;
using Blog.Application.Service.imp;
using Blog.Repository;
using Blog.Repository.Imp;
using Core.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi
{
    public static class Configure
    {
        /// <summary>
        /// 注册Svc
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IWhisperService, WhisperService>();
            services.AddTransient<ICommentService, CommentService>();
            return services;
        }
        /// <summary>
        /// 注册仓储
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<IWhisperRepository, WhisperRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            return services;
        }
        /// <summary>
        /// 注册公共部分
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddTransient<ICacheFactory, CacheFactory>();
            return services;
        }
    }
}
