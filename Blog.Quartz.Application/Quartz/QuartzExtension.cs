using Blog.Quartz.Domain;
using Core.Common.EnumExtensions;
using Microsoft.AspNetCore.Builder;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Quartz.Application.Quartz
{
   public static class QuartzExtension
    {
        /// <summary>
        /// /初始化
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name=""></param>

        public static void InitQuartz(this ISchedulerFactory schedulerFactory,IEnumerable<QuartzOption> list)
        {
            list.ToList().ForEach(x =>
            {
                schedulerFactory.AddJob(x).GetAwaiter().GetResult();
            });
        }
        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="quartzOption"></param>
        /// <returns></returns>
        public static async Task AddJob(this ISchedulerFactory schedulerFactory, QuartzOption quartzOption)
        {
            bool validCron = quartzOption.Cron.ValidCron();
            if (validCron == false)
                throw new ArgumentException("表达式格式错误");
            IJobDetail job= JobBuilder.Create<Job>().WithIdentity(quartzOption.JobName, quartzOption.GroupName).Build();
            ITrigger trigger = TriggerBuilder.Create().WithIdentity(quartzOption.JobName, quartzOption.GroupName).
                StartNow().WithDescription(quartzOption.Description).WithCronSchedule(quartzOption.Cron).Build();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
            await scheduler.PauseTrigger(trigger.Key);
        }
        public static async Task TriggerAction(this ISchedulerFactory schedulerFactory, IEnumerable<QuartzOption> list, JobAction action)
        {
            foreach(var item in list)
            {
                await schedulerFactory.TriggerAction(item, action);
            }
        }
        public static async Task<string> TriggerAction(this ISchedulerFactory schedulerFactory, QuartzOption quartzOption, JobAction action)
        {
            try
            {
                IScheduler scheduler = await schedulerFactory.GetScheduler();
                List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(quartzOption.GroupName)).Result.ToList();
                if (jobKeys == null || jobKeys.Count() == 0)
                {
                    return string.Format("未找到分组：{0}", quartzOption.GroupName);
                }
                JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x => (x as CronTriggerImpl).Name == quartzOption.JobName)).FirstOrDefault();
                if (jobKey == null)
                {
                    return string.Format("未找到触发器：{0}", quartzOption.JobName);
                }
                var triggers = await scheduler.GetTriggersOfJob(jobKey);
                ITrigger trigger = triggers?.Where(x => (x as CronTriggerImpl).Name == quartzOption.JobName).FirstOrDefault();

                if (trigger == null)
                {
                    return string.Format("未找到触发器：{0}", quartzOption.JobName);
                }
                switch (action)
                {
                    case JobAction.删除:
                    case JobAction.修改:
                        await scheduler.PauseTrigger(trigger.Key);
                        await scheduler.UnscheduleJob(trigger.Key);// 移除触发器
                        await scheduler.DeleteJob(trigger.JobKey);
                        break;
                    case JobAction.暂停:
                    case JobAction.停止:
                    case JobAction.开启:
                        if (action == JobAction.暂停)
                        {
                            await scheduler.PauseTrigger(trigger.Key);
                        }
                        else if (action == JobAction.开启)
                        {
                            await scheduler.ResumeTrigger(trigger.Key);
                            //await scheduler.ResumeTrigger(trigger.Key);
                        }
                        else
                        {
                            await scheduler.Shutdown();
                        }
                        break;
                    case JobAction.立即执行:
                        await scheduler.TriggerJob(jobKey);
                        break;
                }
                return string.Format("{0}成功", action.GetEnumText());
            }
            catch (Exception ex)
            {
                return string.Format("{0}失败：", action.GetEnumText(), ex.Message);
            }
        }
        /// <summary>
        /// 验证表达式
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        public static bool ValidCron(this string cron)
        {
            try
            {
                CronTriggerImpl cronTrigger = new CronTriggerImpl();
                cronTrigger.CronExpressionString = cron;
                DateTimeOffset? date = cronTrigger.ComputeFirstFireTimeUtc(null);
                return date != null;
            }
            catch (Exception)
            {

               return false;
            }
            
        }
    }
}
