using Blog.Quartz.Domain;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Quartz.Repository
{
   public interface IQuartzOptionRepository: IRepository<QuartzOption, int>
    {
    }
}
