using Core.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class LeaveMessage: Entity<int>
    {
        /// <summary>
        /// 是否属于友链
        /// </summary>
        public bool IsFriendLink { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content { get;  set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContractEmail { get;  set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool IsAction { get;  set; }
    }
}
