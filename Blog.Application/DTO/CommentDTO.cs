using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.DTO
{
   public class CommentDTO
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论类型
        /// </summary>
        public int CommentType { get; set; }
        /// <summary>
        /// 评论人账号
        /// </summary>
        public string PostUser { get; set; }
        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string PostUsername { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public string PostDate { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string AdditionalData { get; set; }
        /// <summary>
        /// 评论接收人
        /// </summary>
        public string Revicer { get; set; }
        /// <summary>
        /// 评论接收人
        /// </summary>
        public string RevicerName { get; set; }
        /// <summary>
        /// 原内容
        /// </summary>
        public string UsingContent { get; set; }
    }
}
