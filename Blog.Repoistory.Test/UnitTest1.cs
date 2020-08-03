using Blog.Domain.Article;
using Blog.Repoistory.DB;
using Blog.Repoistory.Imp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Blog.Repoistory.Test
{
    public class UnitTest1
    {
        private IArticleRepoistory articleRepoistory;
        public static DbContextOptions<DBContext> db;
        public UnitTest1()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddTransient<IArticleRepoistory, ArticleRepoistory>();
                services.AddDbContext<DbContext>();
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var builder = new DbContextOptionsBuilder<DBContext>();
                builder.UseMySQL("Data Source=58.87.92.221;Database=Blog;User ID=sa;Password=Sa@123456;pooling=true;port=3306;sslmode=none;CharSet=utf8");
                db = builder.Options;
                articleRepoistory = serviceProvider.GetRequiredService<IArticleRepoistory>();
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
                DBContext dBContext = new DBContext(db);
                var v = dBContext.Set<Article>().Find(1);
                Article article = articleRepoistory.SelectById(1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
