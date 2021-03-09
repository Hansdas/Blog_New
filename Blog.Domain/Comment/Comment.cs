using Core.CPlatform.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Domain
{
   public class Comment:Entity<string>
    {
        #region ef忽略字段
        [NotMapped]
        public new string Id { get; set; }
        [NotMapped]
        public new DateTime CreateTime { get; set; }
        [NotMapped]
        public new DateTime? UpdateTime { get; set; }
        #endregion

        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
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
        /// 评论时间
        /// </summary>
        public DateTime PostDate { get;  set; }
    }
}
