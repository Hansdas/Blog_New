using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Cache;
using Core.Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{
    [Route("api/tidings")]
    [ApiController]
    public class TidingsController : ControllerBase
    {
        private ITidingsService _tidingsService;
        private ICacheFactory _cacheFactory;
        private IHttpContextAccessor _httpContext;
        public TidingsController(ITidingsService tidingsService, ICacheFactory cacheFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tidingsService = tidingsService;
            _cacheFactory = cacheFactory;
            _httpContext = httpContextAccessor;
        }
        private UserDTO GetLoginUser()
        {
            Auth auth = new Auth(_cacheFactory, _httpContext);
            UserDTO userDTO = auth.GetLoginUser();
            return userDTO;
        }
        [Route("user")]
        [HttpGet]
        public ApiResult TidingsCount()
        {
            UserDTO userDTO = GetLoginUser();
            TidingsCondition condition = new TidingsCondition();
            condition.IsRead = false;
            condition.Account = userDTO.Account;
            int count = _tidingsService.SelectCount(condition);
            return ApiResult.Success(count);
        }
        [Route("{page}")]
        [HttpPost]
        public ApiResult Tidings([FromBody]TidingsCondition condition)
        {
            UserDTO userDTO = GetLoginUser();
            condition.IsRead = false;
            condition.Account = userDTO.Account;
            List<TidingsDTO> tidingsDTOs= _tidingsService.GetTidingsDTOs(condition.CurrentPage,condition.PageSize, condition);
            return ApiResult.Success(tidingsDTOs);
        }
        [Route("add")]
        [HttpPost]
        public ApiResult CreateTidings([FromBody]TidingsDTO tidingsDTO)
        {
             _tidingsService.Create(tidingsDTO);
            return ApiResult.Success();
        }
    }
}
