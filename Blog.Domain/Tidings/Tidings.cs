using Core.CPlatform.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Domain.Tidings
{
    public class Tidings : Entity<int>
    {
        /// <summary>
        /// 评论id
        /// </summary>
        public string CommentId { get; set; }
        /// <summary>
        /// 评论人
        /// </summary>
        public string PostUser { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string PostContent { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string ReviceUser { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 发送日期
        /// </summary>
        public DateTime SendDate { get; set; }
        /// <summary>
        /// 跳转url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalData { get; set; }
        [NotMapped]
        public new DateTime CreateTime { get; set; }
        [NotMapped]
        public new DateTime? UpdateTime { get; set; }
    }
}
