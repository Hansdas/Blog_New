using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Common.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{

    [Route("api/article")]
    [ApiController]
    public class ArticleController:ControllerBase
    {
        private IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpPost]
        public ApiResult LoadArticlePage([FromBody]ArticleCondition articleCondition)
        {
           IList<ArticleDTO> articleDTOs= _articleService.SelectByPage(articleCondition.CurrentPage,articleCondition.PageSize,articleCondition);
            return ApiResult.Success(articleDTOs);
        }
    }
}
