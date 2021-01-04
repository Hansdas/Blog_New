using Blog.Domain.Article;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Application.Condition
{
   public class ArticleCondition:PageCondition
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int ArticleType { get; set; }
        /// <summary>
        /// 全文索引查询
        /// </summary>
        public string FullText { get; set; }
        /// <summary>
        /// 根据账号查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 标题模糊查询
        /// </summary>
        public string TitleContain { get; set; }
        /// <summary>
        /// 是否是草稿
        /// </summary>
        public string IsDraft { get; set; }
        /// <summary>
        /// 是否根据登录人查询
        /// </summary>
        public bool LoginUser { get; set; } = false;
        public static Expression<Func<Article, bool>> BuildExpression(ArticleCondition condition)
        {
            Expression<Func<Article, bool>> expressLeft = null;
            if (condition.ArticleType != 0)
            {
                Expression<Func<Article, bool>> expressRight = s => s.ArticleType == Enum.Parse<ArticleType>(condition.ArticleType.ToString());
                Expression.Add(expressLeft, expressRight);
            }
            if (!string.IsNullOrEmpty(condition.Account))
            {
                Expression<Func<Article, bool>> expressRight = s => s.Author == condition.Account;
                Expression.Add(expressLeft, expressRight);
            }
            if (!string.IsNullOrEmpty(condition.TitleContain))
            {
                Expression<Func<Article, bool>> expressRight = s => s.Title.Contains(condition.TitleContain);
                Expression.Add(expressLeft, expressRight);
            }
            if (!string.IsNullOrEmpty(condition.IsDraft))
            {
                Expression<Func<Article, bool>> expressRight = s => s.IsDraft== Convert.ToBoolean(condition.IsDraft);
                Expression.Add(expressLeft, expressRight);
            }
            return expressLeft;
        }
    }
}
