using Blog.Quartz.Domain;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Quartz.Repository
{
   public interface IQuartzOptionRepository: IRepository<QuartzOption, int>
    {
        /// <summary>
        /// 批量更新
        /// </summary>
        void UpdateRange(IEnumerable<QuartzOption> quartzOptions);
        /// <summary>
        /// 批量更新
        /// </summary>
        void UpdateStatusAll(TaskStatus taskStatus);
        /// <summary>
        /// 删除所有
        /// </summary>
        void DeleteAll();

    }
}
