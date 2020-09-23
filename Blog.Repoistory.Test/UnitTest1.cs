using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using Blog.Domain;
using Blog.Repository;
using Blog.Repository.Imp;
using Core.Repoistory.DB;

namespace Blog.Repoistory.Test
{
    public class UnitTest1
    {
        public static DbContextOptions<Core.Repoistory.DB.DBContext> db;
        public UnitTest1()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddTransient<IArticleRepository, ArticleRepository>();
                services.AddDbContext<DbContext>();
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var builder = new DbContextOptionsBuilder<DBContext>();
                builder.UseMySQL("Data Source=58.87.92.221;Database=Blog;User ID=sa;Password=Sa@123456;pooling=true;port=3306;sslmode=none;CharSet=utf8");
                db = builder.Options;
            }
            catch (Exception ex)
            {

                //throw ex;
            }
           
        }
        public static DbContextOptions<DBContext> CreateDbContextOptions()
        {
            var serviceProvider = new ServiceCollection().
                AddDbContext<DBContext>()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DBContext>();
          
            return builder.Options;
        }
        [Fact]
        public void Test1()
        {
            try
            {
                DBContext context = new DBContext(db);
                //Article article = context.Set<Article>().Find(1);
                User user = context.Set<User>().Find(1);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
