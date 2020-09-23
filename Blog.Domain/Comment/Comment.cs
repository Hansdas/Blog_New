using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class Comment:Entity<string>
    {
        /// <summary>
        /// 评论类型
        /// </summary>
        public CommentType CommentType { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论人账号
        /// </summary>
        public string PostUser { get;set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string AdditionalData { get;set; }
        /// <summary>
        /// 评论接收人
        /// </summary>
        public string RevicerUser { get;set; }
        /// <summary>
        /// 原文内容
        /// </summary>
        public string UsingContent { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime PostDate { get;  set; }
    }
}
