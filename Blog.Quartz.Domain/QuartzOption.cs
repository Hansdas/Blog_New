using Core.CPlatform.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Quartz.Domain
{
   public class QuartzOption:Entity<int>
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 调用的api
        /// </summary>
        public string Api { get; set; }
        /// <summary>
        /// 最后执行时间
        /// </summary>
        public DateTime? LastActionTime { get; set; } 
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskStatus TaskStatus { get; set; }
    }
}
