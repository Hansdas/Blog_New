using Blog.Domain.Article;
using Blog.Repoistory.DB;
using Blog.Repoistory.Imp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using System.Linq;

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
                builder.UseMySQL("");
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
                DBContext context = new DBContext(db);
                Article article = context.Set<Article>().Find(1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
