using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Sms.Application.DTO
{
   public class WhisperDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// 作者账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 作者名字
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        ///  内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发表日期
        /// </summary>
        public string CreateDate { get; set; }
        public string CommentIds { get; set; }
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
