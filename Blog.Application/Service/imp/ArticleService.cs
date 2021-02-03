using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Domain.Article;
using Blog.Repository;
using Core.Aop;
using Core.Aop.Transaction;
using Core.CPlatform;
using Core.Domain.Core;
using Core.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Blog.Application.Service.imp
{
    public class ArticleService : IArticleService, IInterceptorTag
    {
        private IArticleRepository _articleRepoistory;
        private IUserRepository _userRepoistory;
        private ICommentService _commentService;
        private ICommentRepository _commentRepository;
        private ITidingsService _tidingsService;
        private IHttpClientFactory _httpClientFactory;
        public ArticleService(IArticleRepository articleRepoistory, IUserRepository userRepoistory, ICommentService commentService
            , ICommentRepository commentRepository, ITidingsService tidingsService, IHttpClientFactory httpClientFactory)
        {
            _articleRepoistory = articleRepoistory;
            _userRepoistory = userRepoistory;
            _commentService = commentService;
            _commentRepository = commentRepository;
            _tidingsService = tidingsService;
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// 抓换查询条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
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
        private Article ConvertToArticle(ArticleDTO articleDTO)
        {
            Article article = new Article();
            article.Author = articleDTO.AuthorAccount;
            article.Title = articleDTO.Title;
            article.IsDraft = articleDTO.IsDraft;
            article.Content = articleDTO.Content;
            article.ArticleType = Enum.Parse<ArticleType>(articleDTO.ArticleType);
            article.TextSection = articleDTO.ContentBriefly;
            article.CreateTime = DateTime.Now;
            return article;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="articleDTO"></param>
        /// <returns></returns>
        public int Add(ArticleDTO articleDTO)
        {
            Article article = ConvertToArticle(articleDTO);
            article = _articleRepoistory.Insert(article);
            return article.Id;
        }
        /// <summary>
        /// 根据阅读量分组
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int SelectCount(ArticleCondition condition = null)
        {
            int count = _articleRepoistory.SelectCountWithDapper(ConvertCondition(condition));
            return count;
        }
        /// <summary>
        /// 查询热门文章
        /// </summary>
        /// <returns></returns>
        public IList<ArticleDTO> SelectHotList()
        {
            Expression<Func<Article, object>> orderBy = s => s.BrowserCount;
            IList<Article> articles = _articleRepoistory.SelectTop(5, null, orderBy, true);
            IList<ArticleDTO> articleDTOs = new List<ArticleDTO>();
            foreach (var item in articles)
            {
                ArticleDTO articleDTO = new ArticleDTO();
                articleDTO.Id = item.Id;
                articleDTO.ArticleType = Enum.GetName(typeof(ArticleType), item.ArticleType);
                articleDTO.Title = item.Title;
                articleDTO.ReadCount = item.BrowserCount;
                articleDTOs.Add(articleDTO);
            }
            return articleDTOs;
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleDTO SelectById(int id)
        {
            Article article = _articleRepoistory.SelectById(id);
            User user = _userRepoistory.SelectSingle(s => s.Account == article.Author);
            ArticleDTO articleDTO = new ArticleDTO();
            if (article.Content.Contains(ConstantKey.NGINX_FILE_ROUTE_OLD))
                article.Content = article.Content.Replace(ConstantKey.NGINX_FILE_ROUTE_OLD, ConstantKey.NGINX_FILE_ROUTE);
            articleDTO.ArticleType = Enum.GetName(typeof(ArticleType), article.ArticleType);
            articleDTO.Title = article.Title;
            articleDTO.AuthorAccount = article.Author;
            articleDTO.AuthorName = user.Username;
            articleDTO.Content = article.Content;
            articleDTO.CreateDate = article.CreateTime.ToString("yyyy-MM-dd HH:mm");
            articleDTO.ReviewCount = article.CommentCount;
            articleDTO.PraiseCount = article.PraiseCount;
            articleDTO.ReadCount = article.BrowserCount;
            IList<CommentDTO> comments = _commentService.Select(article.CommentIdList.Distinct());
            articleDTO.Comments = comments == null ? new List<CommentDTO>() : comments;
            article.BrowserCount += 1;
            _articleRepoistory.Update(article);
            return articleDTO;
        }

        public IList<CommentDTO> SelectComments()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询上一篇下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public UpNextDto SelectContext(int id, ArticleCondition condition = null)
        {
            IEnumerable<dynamic> dynamics = _articleRepoistory.SelectContext(id, ConvertCondition(condition));
            UpNextDto pageInfoMode = new UpNextDto();
            foreach (dynamic d in dynamics)
            {
                if (d.article_id > id)
                {
                    pageInfoMode.NextId = d.article_id;
                    pageInfoMode.NextTitle = d.article_title;
                }
                else
                {
                    pageInfoMode.BeforeId = d.article_id;
                    pageInfoMode.BeforeTitle = d.article_title;
                }
            }
            return pageInfoMode;
        }
        /// <summary>
        /// 提交评论
        /// </summary>
        public async Task PostComment(int articleId, CommentDTO commentDTO)
        {
            Article article = _articleRepoistory.SelectById(articleId);
            List<string> commentIds = article.CommentIdList;
            Comment comment = new Comment();
            comment.Guid = Guid.NewGuid().ToString();
            comment.Content = commentDTO.Content;
            comment.PostUser = commentDTO.PostUser;
            comment.RevicerUser = string.IsNullOrEmpty(commentDTO.Revicer) ? article.Author : commentDTO.Revicer;
            comment.CommentType = Enum.Parse<CommentType>(commentDTO.CommentType.ToString());
            comment.PostDate = DateTime.Now;
            comment.AdditionalData = commentDTO.ToGuid;

            commentIds.Add(comment.Guid);
            TidingsDTO tidingsDTO = new TidingsDTO();
            tidingsDTO.PostUserAccount = comment.PostUser;
            tidingsDTO.PostContent = comment.Content;
            tidingsDTO.ReviceUserAccount = comment.RevicerUser;
            tidingsDTO.Url = string.Format("../article/detail.html?id={0}", articleId);
            tidingsDTO.PostDate = DateTime.Now.ToString();
            tidingsDTO.IsRead = false;
            tidingsDTO.CommentId = comment.Guid;
            if (comment.CommentType == CommentType.评论)
            {
                Comment toComment = _commentRepository.SelectById(commentDTO.ToGuid);
                tidingsDTO.Content = toComment.Content;
            }
            else
                tidingsDTO.Content = article.Title;
            using (TransactionScope trans = new TransactionScope())
            {
                try
                {
                    _commentRepository.Insert(comment);
                    _articleRepoistory.UpdateCommentIds(commentIds, articleId);
                    _tidingsService.Create(tidingsDTO);
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    LogUtils.LogError(ex, "ArticleService.PostComment", ex.Message);
                    throw ex;
                }
            }

            TidingsCondition tidingsCondition = new TidingsCondition();
            tidingsCondition.Account = tidingsDTO.ReviceUserAccount;
            tidingsCondition.IsRead = false;
            int count = _tidingsService.SelectCount(tidingsCondition);
            HttpClient httpClient = _httpClientFactory.CreateClient();
            Dictionary<string, string> requestContents = new Dictionary<string, string>();
            requestContents.Add("user", tidingsDTO.ReviceUserAccount);
            requestContents.Add("count", count.ToString());
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(requestContents);
            IHttpContextAccessor httpContext = ServiceLocator.Get<IHttpContextAccessor>();
            httpContext.HttpContext.Request.Headers.TryGetValue("authorization", out StringValues authorization);
            httpClient.DefaultRequestHeaders.Add("Authorization", authorization.ToString());
            HttpResponseMessage responseMessage = await httpClient.PostAsync(ConstantKey.GATEWAY_HOST + "/sms/singlar/tidings/count", formUrlEncodedContent);
            if (!responseMessage.IsSuccessStatusCode)
            {
                //LogUtils.LogError(ex, "ArticleService.PostComment", ex.Message);
            }
            var v=await responseMessage.Content.ReadAsStringAsync();

        }

        public List<ArticleFileDTO> SelectArticleFile(ArticleCondition articleCondition)
        {
            articleCondition.IsDraft = "false";
            IEnumerable<dynamic> resultList = _articleRepoistory.SelectArticleFile(ConvertCondition(articleCondition));
            List<ArticleFileDTO> fileModels = new List<ArticleFileDTO>();
            foreach (var item in resultList)
            {
                ArticleFileDTO model = new ArticleFileDTO();
                model.ArticleType = item.article_articletype;
                model.Total = (int)item.count;
                model.Account = item.article_author;
                fileModels.Add(model);
            }
            return fileModels;
        }

        public void DeleteById(int id)
        {
            _articleRepoistory.DeleteById(id);
        }
    }
}
