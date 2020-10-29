using Blog.Domain.Core;
using Core.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
  public  class Whisper:Entity<int>
    {
        /// <summary>
        /// 提交人
        /// </summary>
        public string Account { get;  set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get;  set; }
        /// <summary>
        /// 评论guid(只用来数据持久化)
        /// </summary>
        public string CommentGuids { get;  set; }
        /// <summary>
        /// 该条数据审核是否通过
        /// </summary>
        public bool? IsPassing { get; private set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount
        {
            get
            {
                if (string.IsNullOrEmpty(CommentGuids))
                    return 0;
                return CommentGuids.Split(",").Length;
            }
        }
    }
}
