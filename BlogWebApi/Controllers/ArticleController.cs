using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Common.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{

    [Route("api/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        [Route("{id}")]
        public ApiResult LoadArticle(int id)
        {
            ArticleDTO articleDTO = _articleService.SelectById(id);
            return ApiResult.Success(articleDTO);
        }
        [HttpPost]
        public ApiResult LoadArticlePage([FromBody]ArticleCondition articleCondition)
        {
            IList<ArticleDTO> articleDTOs = _articleService.SelectByPage(articleCondition.CurrentPage, articleCondition.PageSize, articleCondition);
            return ApiResult.Success(articleDTOs);
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
    }
}
