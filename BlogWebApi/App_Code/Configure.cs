using Blog.Application.Service;
using Blog.Application.Service.imp;
using Blog.Repository;
using Blog.Repository.Imp;
using Core.Auth.IdentityServer4;
using Core.Cache;
using Core.Common.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IWhisperService, WhisperService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ILeaveMessageService, LeaveMessageService>();
            services.AddScoped<ITidingsService, TidingsService>();
            return services;
        }
        /// <summary>
        /// 注册仓储
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IWhisperRepository, WhisperRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILeaveMessageRepository, LeaveMessageRepository>();
            services.AddScoped<ITidingsRepository, TidingsRepository>();
            return services;
        }
        /// <summary>
        /// 注册公共部分
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            services.AddSingleton<ICacheFactory, CacheFactory>();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            services.AddMvc(s => s.Filters.Add<GlobaExceptionFilterAttribute>());
            // services.AddAuth();
            return services;
        }
    }
}
