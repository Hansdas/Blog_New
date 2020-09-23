using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Article;
using Blog.Repoistory.DB;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using Blog.Repository.DB;

namespace Blog.Repository.Imp
{
    public class ArticleRepository :Repository<Article,int>, IArticleRepository
    {
        private IDbConnection connection => DapperContent.connection();
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
                parameters.Add("isDraft", condition["isDraft"]);
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
        public int SelectCountWithDapper(Hashtable condition=null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(condition, ref parameters);
            string sql = "SELECT COUNT(*) FROM T_Article WHERE " + where;
            int count = connection.ExecuteScalar<int>(sql, parameters); 
            return count;
        }
        public IEnumerable<Article> SelectByPageWithDapper(int currentPage, int pageSize, Hashtable condition = null)
        {
            int pageId = pageSize * (currentPage - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId, DbType.Int32);
            parameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition, ref parameters);
            string sql = "SELECT article_id,article_author,article_title,article_textsection,article_articletype,article_isdraft,article_praisecount,article_browsercount,article_comments,article_createtime " +
                  "FROM T_Article  WHERE " + where +
                  " ORDER BY article_createtime DESC LIMIT @pageId,@pageSize";

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

     

        public IList<Article> SelectTop(int top, Expression<Func<Article, bool>> where=null, Expression<Func<Article, object>> orderBy = null)
        {
            IQueryable<Article> queryable= AsQueryable(where, orderBy).Take(top);
            var results = from a in queryable
                          select new
                          {
                              a.Id,
                              a.Title,
                              a.ArticleType,
                          };
            IList<Article> articles = new List<Article>();
            foreach (var result in results)
            {
                Article article = new Article();
                article.Id = result.Id;
                article.Title = result.Title;
                article.ArticleType = result.ArticleType;
                articles.Add(article);
            }

            return articles;
        }
    }
}
