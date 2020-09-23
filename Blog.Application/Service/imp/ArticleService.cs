using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Domain.Article;
using Blog.Repository;
using Blog.Repository.Imp;

namespace Blog.Application.Service.imp
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepoistory;
        private IUserRepository _userRepoistory;
        public ArticleService(IArticleRepository articleRepoistory, IUserRepository userRepoistory)
        {
            _articleRepoistory = articleRepoistory;
            _userRepoistory = userRepoistory;
        }
        private Hashtable ConvertCondition(ArticleCondition condition)
        {
            Hashtable hashtable = new Hashtable();
            if (condition == null)
                return hashtable;
            if (condition.ArticleType != 0)
                hashtable.Add("articleType", condition.ArticleType);
            if (!string.IsNullOrEmpty(condition.Account))
                hashtable.Add("author", condition.Account);
            if (!string.IsNullOrEmpty(condition.TitleContain))
                hashtable.Add("titleContain", condition.TitleContain);
            if (!string.IsNullOrEmpty(condition.IsDraft))
                hashtable.Add("isDraft", condition.IsDraft);
            if (!string.IsNullOrEmpty(condition.FullText))
                hashtable.Add("fullText", condition.FullText);
            return hashtable;
        }
        public IList<ArticleDTO> GetGroupReadCount()
        {
            IList<Article> articles = _articleRepoistory.SelectGroupReadCount();
            IEnumerable<string> accounts = articles.Select(s => s.Author);
            Dictionary<string, string> accountWithName = _userRepoistory.AccountWithName(accounts);
            IList<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (var item in articles)
            {
                ArticleDTO articleDTO = new ArticleDTO();
                articleDTO.Id = item.Id;
                articleDTO.ArticleType = Enum.GetName(typeof(ArticleType), item.ArticleType);
                articleDTO.Title = item.Title;
                articleDTO.AuthorAccount = item.Author;
                articleDTO.AuthorName = accountWithName[item.Author];
                articleDTO.ContentBriefly = item.TextSection;
                articleDTO.CreateDate = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                articleDTO.ReviewCount = item.CommentCount;
                articleDTO.PraiseCount = item.PraiseCount;
                articleDTO.ReadCount = item.BrowserCount;
                articleDTOs.Add(articleDTO);
            }
            return articleDTOs;
        }

        public IList<ArticleDTO> SelectByPage(int currentPage, int pageSize, ArticleCondition condition = null)
        {
            IEnumerable<Article> articles = _articleRepoistory.SelectByPageWithDapper(currentPage, pageSize, ConvertCondition(condition));
            IEnumerable<string> accounts = articles.Select(s => s.Author);
            Dictionary<string, string> accountWithName = _userRepoistory.AccountWithName(accounts);
            IList<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (var item in articles)
            {
                ArticleDTO articleDTO = new ArticleDTO();
                articleDTO.Id = item.Id;
                articleDTO.ArticleType = Enum.GetName(typeof(ArticleType), item.ArticleType);
                articleDTO.Title = item.Title;
                articleDTO.AuthorAccount = item.Author;
                articleDTO.AuthorName = accountWithName[item.Author];
                articleDTO.ContentBriefly = item.TextSection;
                articleDTO.CreateDate = item.CreateTime.ToString("yyyy-MM-dd HH:mm");
                articleDTO.ReviewCount = item.CommentCount;
                articleDTO.PraiseCount = item.PraiseCount;
                articleDTO.ReadCount = item.BrowserCount;
                articleDTOs.Add(articleDTO);
            }
            return articleDTOs;
        }

        public int SelectCount(ArticleCondition condition = null)
        {
            int count = _articleRepoistory.SelectCountWithDapper(ConvertCondition(condition));
            return count;
        }
        public IList<ArticleDTO> SelectHotList()
        {
            Expression<Func<Article, object>> orderBy = s => s.BrowserCount;
            IList<Article> articles = _articleRepoistory.SelectTop(5, null, orderBy);
            IList<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (var item in articles)
            {
                ArticleDTO articleDTO = new ArticleDTO();
                articleDTO.Id = item.Id;
                articleDTO.ArticleType = Enum.GetName(typeof(ArticleType), item.ArticleType);
                articleDTO.Title = item.Title;
                articleDTOs.Add(articleDTO);
            }
            return articleDTOs;
        }
        public ArticleDTO SelectById(int id)
        {
            Article article = _articleRepoistory.SelectById(id);
            User user = _userRepoistory.SelectSingle(s => s.Account == article.Author);
            ArticleDTO articleDTO = new ArticleDTO();
            articleDTO.ArticleType = Enum.GetName(typeof(ArticleType), article.ArticleType);
            articleDTO.Title = article.Title;
            articleDTO.AuthorAccount = article.Author;
            articleDTO.AuthorName = user.Username;
            articleDTO.Content = article.Content;
            articleDTO.CreateDate = article.CreateTime.ToString("yyyy-MM-dd HH:mm");
            articleDTO.ReviewCount = article.CommentCount;
            articleDTO.PraiseCount = article.PraiseCount;
            articleDTO.ReadCount = article.BrowserCount;
            return articleDTO;
        }

        public IList<CommentDTO> SelectComments()
        {
            throw new NotImplementedException();
        }
    }
}
