using Blog.Application.Condition;
using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Service
{
   public interface IArticleService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="articleDTO"></param>
        /// <returns></returns>
        int Add(ArticleDTO articleDTO);
        /// <summary>
        /// 根据阅读量分组
        /// </summary>
        /// <returns></returns>
        IList<ArticleDTO> GetGroupNewCount();
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
        /// <summary>
        /// 查询上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        UpNextDto SelectContext(int id, ArticleCondition condition = null);

        /// <summary>
        /// 提交留言评论
        /// </summary>
        Task PostComment(int articleId, CommentDTO commentDTO);
        /// <summary>
        /// 查询文章归档
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        List<ArticleFileDTO> SelectArticleFile(ArticleCondition articleCondition);
        /// <summary>
        /// 根据id删除
        /// </summary>
        void DeleteById(int id);
    }
}
