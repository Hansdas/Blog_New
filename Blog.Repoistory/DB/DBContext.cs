
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repoistory.DB
{
   public class DBContext:DbContext
    {
        public DBContext()
        {

        }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
            builder.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new UserMap());
            //modelBuilder.ApplyConfiguration(new ArticleMap());
            //modelBuilder.ApplyConfiguration(new WhisperMap());
        }
    }
}
