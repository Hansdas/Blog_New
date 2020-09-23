using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.DTO
{
   public class ArticleDTO
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 作者名字
        /// </summary>
        public string AuthorName { get; set; }
        /// <summary>
        /// 作者账号
        /// </summary>
        public string AuthorAccount { get; set; }
        /// <summary>
        /// 作者头像
        /// </summary>
        public string AuthorPhoto { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文章简要
        /// </summary>
        public string ContentBriefly { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        public string ArticleType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 留言数量
        /// </summary>
        public int ReviewCount { get; set; }
        /// <summary>
        /// 阅读数量
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int PraiseCount { get; set; }
    }
}
