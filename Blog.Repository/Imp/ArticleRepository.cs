using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Article;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using Blog.Repository.DB;
using Core.Repository.Imp;

namespace Blog.Repository.Imp
{
    public class ArticleRepository :Repository<Article,int>, IArticleRepository
    {
        private IDbConnection connection => DapperContext.connection();
        public ArticleRepository(DBContext dbContext) : base(dbContext)
        {

        }
        /// <summary>                          w
        /// 拼接查询条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dynamicParameters"></param>
        /// <returns></returns>
        private string Where(Hashtable condition, ref DynamicParameters parameters)
        {
            IList<string> sqlList = new List<string>();
            if (condition.ContainsKey("articleType"))
            {
                parameters.Add("articleType", condition["articleType"]);
                sqlList.Add("article_articletype = @articleType");
            }
            if (condition.ContainsKey("author"))
            {
                parameters.Add("author", condition["author"]);
                sqlList.Add("article_author = @author");
            }
            if (condition.ContainsKey("titleContain"))
            {
                parameters.Add("titleContain", condition["titleContain"]);
                sqlList.Add("article_title like CONCAT('%',@titleContain,'%')");
            }
            if (condition.ContainsKey("isDraft"))
            {
                parameters.Add("isDraft",Convert.ToBoolean(condition["isDraft"]),DbType.Boolean);
                sqlList.Add("article_isdraft = @isDraft");
            }
            if (condition.ContainsKey("fullText"))
            {
                parameters.Add("fullText", condition["fullText"]);
                sqlList.Add("MATCH(article_title,article_content) AGAINST(@fullText IN BOOLEAN MODE)");
            }
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
        }
        /// <summary>
        /// 查询每组最大浏览量的数据
        /// </summary>
        /// <returns></returns>
        public  IList<Article> SelectGroupReadCount()
        {
            string sql = "select article_id,article_title,article_author,article_textsection,article_articletype,article_createtime " +
                "from T_Article a where not exists(select * from T_Article where a.article_browsercount<article_browsercount and a.article_articletype=article_articletype )";
            IEnumerable<dynamic> results = connection.Query<dynamic>(sql).ToList();
            IList<Article> articles = new List<Article>();
            foreach(var d in results)
            {
                Article article = new Article();
                article.Id = d.article_id;
                article.Title = d.article_title;
                article.Author = d.article_author;
                article.TextSection = d.article_textsection;
                article.ArticleType = (ArticleType)d.article_articletype;
                article.CreateTime = d.article_createtime;
                articles.Add(article);
            }
            return articles;

        }
        /// <summary>
        /// 通过dapper查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int SelectCountWithDapper(Hashtable condition=null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(condition, ref parameters);
            string sql = "select count(*) from T_Article where " + where;
            int count = connection.ExecuteScalar<int>(sql, parameters); 
            return count;
        }
        /// <summary>
        /// 通过dapper分页查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<Article> SelectByPageWithDapper(int currentPage, int pageSize, Hashtable condition = null)
        {
            int pageId = pageSize * (currentPage - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId, DbType.Int32);
            parameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition, ref parameters);
            string sql = "select article_id,article_author,article_title,article_textsection,article_articletype,article_isdraft,article_praisecount,article_browsercount,article_comments,article_createtime " +
                  "from T_Article  where " + where +
                  " order by article_createtime desc limit @pageId,@pageSize";

            IEnumerable<dynamic> dynamics = connection.Query(sql, parameters);
            IList<Article> articles = new List<Article>();
            foreach (var d in dynamics)
            {

                Article article = new Article();
                article.Id = d.article_id;
                article.Author = d.article_author;
                article.TextSection = d.article_textsection;
                article.Title = d.article_title;
                article.ArticleType = (ArticleType)d.article_articletype;
                article.IsDraft = Convert.ToBoolean(d.article_isdraft);
                article.PraiseCount = d.article_praisecount;
                article.BrowserCount = d.article_browsercount;
                article.CommentIds = d.article_comments;
                article.CreateTime = (DateTime)d.article_createtime;
                articles.Add(article);
            }
            return articles;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public override IEnumerable<Article> SelectByPage(int currentPage, int pageSize, Expression<Func<Article, bool>> where = null, Expression<Func<Article, object>> orderBy = null)
        {
            int pageId = (currentPage - 1) * pageSize;
            base.desc = true;
            orderBy = s => s.CreateTime;
            IQueryable<Article> queryable = AsQueryable(where, orderBy).Skip(pageId).Take(pageSize);
            var results = from a in queryable
                          select new
                          {
                              a.Id,
                              a.Author,
                              a.TextSection,
                              a.Title,
                              a.ArticleType,
                              a.IsDraft,
                              a.PraiseCount,
                              a.BrowserCount,
                              a.CommentIds,
                              a.CreateTime
                          };
            IList<Article> articles = new List<Article>();
            foreach (var result in results)
            {
                Article article = new Article();
                article.Id = result.Id;
                article.Author = result.Author;
                article.TextSection = result.TextSection;
                article.Title = result.Title;
                article.ArticleType = result.ArticleType;
                article.IsDraft = result.IsDraft;
                article.PraiseCount = result.PraiseCount;
                article.BrowserCount = result.BrowserCount;
                article.CommentIds = result.CommentIds;
                article.CreateTime = result.CreateTime;
                articles.Add(article);
            }

            return articles;
        }
        /// <summary>
        /// 查询top
        /// </summary>
        /// <param name="top"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public IList<Article> SelectTop(int top, Expression<Func<Article, bool>> where=null, Expression<Func<Article, object>> orderBy = null,bool desc=false)
        {
            this.desc = desc;
            IQueryable<Article> queryable= AsQueryable(where, orderBy).Take(top);
            var results = from a in queryable
                          select new
                          {
                              a.Id,
                              a.Title,
                              a.ArticleType,
                              a.BrowserCount
                          };
            IList<Article> articles = new List<Article>();
            foreach (var result in results)
            {
                Article article = new Article();
                article.Id = result.Id;
                article.Title = result.Title;
                article.ArticleType = result.ArticleType;
                article.BrowserCount = result.BrowserCount;
                articles.Add(article);
            }

            return articles;
        }
        /// <summary>
        /// 查询上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectContext(int id, Hashtable articleCondition = null)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("article_id", id, DbType.Int32);
            string where = Where(articleCondition, ref dynamicParameters);
            string sql = "select article_id,article_title from T_Article "
                       + "where article_id in( "
                             + "select max(article_id) "
                             + "from T_Article "
                             + "where article_id <@article_id  and " + where
                             + "union "
                             + "select min(article_id) "
                             + "from T_Article "
                             + "where article_id >@article_id  and " + where
                             + ")";
            IEnumerable<dynamic> dynamics = connection.Query(sql, dynamicParameters);
            return dynamics;
        }
        /// <summary>
        /// 根据id查询评论集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> SelectCommentIds(int id)
        {
            Expression<Func<Article, bool>> where = s => s.Id == id;
            IQueryable<Article> queryable =  AsQueryable(where) ;
            string commentids=  (from a in queryable select a.CommentIds).FirstOrDefault();
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(commentids))
                list = commentids.Split(",").ToList();
            return list;
        }
        /// <summary>
        /// 更新评论id字段
        /// </summary>
        /// <param name="commentIds"></param>
        /// <param name="id"></param>
        public void UpdateCommentIds(List<string> commentIds, int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Comments", string.Join(',', commentIds));
            parameters.Add("Id", id);
            string sql = "update T_Article set article_comments = @Comments where article_id =@Id";
            connection.Execute(sql, parameters);
        }
        /// <summary>
        /// 查询文章归档
        /// </summary>
        /// <param name="articleCondition"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> SelectArticleFile(Hashtable articleCondition)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(articleCondition, ref parameters);
            string sql = "select count(*) as count,article_author,article_articletype from T_Article where " +
                "" + where + "  group by article_author,article_articletype";
            IEnumerable<dynamic> resultList = connection.Query(sql, parameters);
            return resultList;
        }
    }
}
