using Blog.Domain.Article;
using Core.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Repository
{
    public interface IArticleRepository : IRepository<Article, int>
    {
        /// <summary>
        /// 根据阅读量分组
        /// </summary>
        /// <returns></returns>
        IList<Article> SelectGroupNewCount();
        /// <summary>
        /// 查询阅读量前5
        /// </summary>
        /// <returns></returns>
        IList<Article> SelectTop(int top, Expression<Func<Article, bool>> where = null, Expression<Func<Article, object>> orderBy = null,bool desc = false);
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int SelectCountWithDapper(Hashtable condition = null);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IEnumerable<Article> SelectByPageWithDapper(int currentPage, int pageSize, Hashtable condition = null);
        /// <summary>
        /// 查询上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectContext(int id, Hashtable articleCondition = null);
        /// <summary>
        /// 根据id查询commetId集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<string> SelectCommentIds(int id);
        /// <summary>
        /// 更新评论id字段
        /// </summary>
        /// <param name="commentIds"></param>
        /// <param name="id"></param>
        void UpdateCommentIds(List<string> commentIds,int id);
        /// <summary>
        /// 查询文章归档
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        IEnumerable<dynamic> SelectArticleFile(Hashtable articleCondition);
    }
}
