using Blog.Domain.Core;
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
        private string _authorPhoto;
        /// <summary>
        /// 作者头像
        /// </summary>
        public string AuthorPhoto 
        {
            get
            {
                if (string.IsNullOrEmpty(_authorPhoto))
                    _authorPhoto = "/style/images/touxiang.jpg";
                if (_authorPhoto.Contains(ConstantKey.NGINX_FILE_ROUTE_OLD))
                    _authorPhoto = _authorPhoto.Replace(ConstantKey.NGINX_FILE_ROUTE_OLD, ConstantKey.NGINX_FILE_ROUTE);
                if (_authorPhoto.Contains(ConstantKey.OLD_FILE_HTTP))
                    _authorPhoto = _authorPhoto.Replace(ConstantKey.OLD_FILE_HTTP, ConstantKey.FILE_HTTPS);
                if (_authorPhoto.Contains("http") && !_authorPhoto.Contains("https"))
                    _authorPhoto = _authorPhoto.Replace("http", "https");
                return _authorPhoto;
            }
            set
            {
                _authorPhoto = value;
            }
        }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文章简要
        /// </summary>
        public string ContentBriefly { get; set; }
        private string _contet;
        /// <summary>
        ///  内容
        /// </summary>
        public string Content
        {
            get
            {
                if (string.IsNullOrEmpty(_contet))
                    return "";
                if (_contet.Contains(ConstantKey.OLD_FILE_HTTP))
                    _contet = _contet.Replace(ConstantKey.OLD_FILE_HTTP, ConstantKey.FILE_HTTPS);
                return _contet;
            }
            set
            {
                _contet = value;
            }

        }
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
        /// <summary>
        /// 评论集合
        /// </summary>
        public IList<CommentDTO> Comments { get; set; }
    }
    /// <summary>
    /// 上一篇下一篇模型
    /// </summary>
    public class UpNextDto
    {
        /// <summary>
        /// 上一篇
        /// </summary>
        public int BeforeId { get; set; }
        /// <summary>
        /// 下一篇
        /// </summary>
        public int NextId { get; set; }
        /// <summary>
        /// 上一篇标题
        /// </summary>
        public string BeforeTitle { get; set; }
        /// <summary>
        /// 下一篇标题
        /// </summary>
        public string NextTitle { get; set; }
    }
}
