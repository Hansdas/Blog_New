using Blog.Application.Condition;
using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Service
{
   public interface IArticleService
    {
        /// <summary>
        /// 根据阅读量分组
        /// </summary>
        /// <returns></returns>
        IList<ArticleDTO> GetGroupReadCount();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<ArticleDTO> SelectByPage(int currentPage, int pageSize, ArticleCondition condition=null);
        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int SelectCount(ArticleCondition condition = null);
        /// <summary>
        /// 查询热门推荐
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<ArticleDTO> SelectHotList();

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ArticleDTO SelectById(int id);
        /// <summary>
        /// 查询评论
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<CommentDTO> SelectComments();
    }
}
