using Blog.Quartz.Domain;
using Blog.Quartz.Repository.DB;
using Core.Repository.Imp;
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
    }
}
