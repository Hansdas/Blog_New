using Blog.Domain.Core;
using Core.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Article
{
   public class Article:Entity<int>
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文本截取显示
        /// </summary>
        public string TextSection { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 专栏
        /// </summary>
        public ArticleType ArticleType { get; set; }
        /// <summary>
        /// 是否为草稿
        /// </summary>
        public bool IsDraft { get; set; }
        /// <summary>
        /// 有人评论是否发送邮件
        /// </summary>
        public bool? IsSendEmail { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int PraiseCount { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int BrowserCount { get; set; }
        /// <summary>
        /// 评论 ids
        /// </summary>
        public string CommentIds { get; set; }
        /// <summary>
        /// 评论 ids
        /// </summary>
        public IList<string> CommentIdList {
            get
            {
                if (string.IsNullOrEmpty(CommentIds))
                    return new List<string>();
                return CommentIds.Split(',');
            }
        }
        public int CommentCount
        {
            get
            {
                if (string.IsNullOrEmpty(CommentIds))
                    return 0;
                return CommentIds.Split(',').Length;
            }
        }
    }
}
