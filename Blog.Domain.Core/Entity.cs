using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
   public abstract class Entity<T>
    {
        public  T Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
