using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Cache;
using Core.Common;
using Core.Common.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{

    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleService _articleService;
        private IHttpContextAccessor _httpContext;
        private ICacheFactory _cacheFactory;
        public ArticleController(IArticleService articleService, IHttpContextAccessor httpContext, ICacheFactory cacheFactory)
        {
            _articleService = articleService;
            _httpContext = httpContext;
            _cacheFactory = cacheFactory;
        }
        [HttpGet]
        [Route("{id}")]
        public ApiResult LoadArticle(int id)
        {
            ArticleDTO articleDTO = _articleService.SelectById(id);
            return ApiResult.Success(articleDTO);
        }
        [HttpPost]
        [Route("page")]
        public ApiResult LoadArticlePage([FromBody]ArticleCondition articleCondition)
        {
            if (articleCondition.LoginUser)
                articleCondition.Account = new Auth(_cacheFactory, _httpContext).GetLoginUser().Account;
            IList<ArticleDTO> articleDTOs = _articleService.SelectByPage(articleCondition.CurrentPage, articleCondition.PageSize, articleCondition);
            int count= _articleService.SelectCount(articleCondition);
            return ApiResult.Success(new { list= articleDTOs ,total=count});
        }
        [HttpGet]
        [Route("group/readcount")]
        public ApiResult GroupByReadcount()
        {
            IList<ArticleDTO> articleDTOs = _articleService.GetGroupReadCount();
            return ApiResult.Success(articleDTOs);
        }
        [HttpGet]
        [Route("context/{id}")]
        public ApiResult GetUpNext(int id)
        {
             UpNextDto upNextDto= _articleService.SelectContext(id);
            return ApiResult.Success(upNextDto);
        }
        [HttpGet]
        [Route("top/read")]
        public ApiResult LoadTopRead()
        {
            IList<ArticleDTO> articleDTOs = _articleService.SelectHotList();
            return ApiResult.Success(articleDTOs);
        }
        [HttpPost]
        [Route("comment/add")]
        public async Task<ApiResult> PostComment()
        {
            string content = Request.Form["content"];
            int articleId = Convert.ToInt32(Request.Form["articleId"]);
            string revicer = Request.Form["revicer"];
            string replyId = Request.Form["replyId"];
            int commentType = Convert.ToInt32(Request.Form["commentType"]);
            try
            {
                CommentDTO commentDTO = new CommentDTO();
                commentDTO.Content = content;
                commentDTO.AdditionalData = replyId;
                commentDTO.PostUser = new Auth(_cacheFactory,_httpContext).GetLoginUser().Account;
                commentDTO.Revicer = revicer;
                commentDTO.CommentType = commentType;
                _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);
                await _articleService.PostComment(articleId, commentDTO,value);
                return ApiResult.Success();
            }
            catch (AuthException)
            {
                return ApiResult.Error(HttpStatusCode.FORBIDDEN, "not login");
            }
        }
        /// <summary>
        /// 查询个人归档
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user/archive")]        
        public ApiResult GetArchive()
        {
            Auth auth = new Auth(_cacheFactory, _httpContext);
            UserDTO userDTO = auth.GetLoginUser();
            ArticleCondition articleCondition = new ArticleCondition();
            articleCondition.IsDraft = "false";
            articleCondition.Account = userDTO.Account;
            List<ArticleFileDTO> articleDTOs = _articleService.SelectArticleFile(articleCondition);
            return ApiResult.Success(articleDTOs);
        }
    }
}
