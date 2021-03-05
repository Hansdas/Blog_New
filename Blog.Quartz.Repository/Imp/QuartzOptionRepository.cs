using Blog.Quartz.Domain;
using Blog.Quartz.Repository.DB;
using Core.Common.EnumExtensions;
using Core.Repository.Imp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Quartz.Repository.Imp
{
   public class QuartzOptionRepository: Repository<QuartzOption, int>, IQuartzOptionRepository
    {
        public QuartzOptionRepository(DBContext dbContext) : base(dbContext)
        {

        }

        public void UpdateStatusAll(TaskStatus taskStatus)
        {            
            _dbContext.Database.ExecuteSqlRaw("update SYS_Quartz set task_status={0}",taskStatus.GetEnumValue());
        }

        public void UpdateRange(IEnumerable<QuartzOption> quartzOptions)
        {
            _dbContext.UpdateRange(quartzOptions);
            _dbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _dbContext.Database.ExecuteSqlRaw("truncate table SYS_Quartz");
        }
    }
}
