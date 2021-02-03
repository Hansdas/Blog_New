using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Cache;
using Core.Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi
{
    [Route("api/whisper")]
    [ApiController]
    public class WhisperController: ControllerBase
    {
        private IWhisperService _whisperService;
        private IHttpContextAccessor _httpContext;
        public WhisperController(IWhisperService whisperService, IHttpContextAccessor httpContextAccessor,ICacheFactory cacheFactory)
        {
            _whisperService = whisperService;
            _httpContext = httpContextAccessor;
        }
        [Route("square")]
        [HttpGet]
        public async Task<ApiResult> LoadSquare()
        {
          IList<WhisperDTO> whisperDTOs= await _whisperService.SelectPageCache(0,6);
            return ApiResult.Success(whisperDTOs);
        }
        [Route("add")]
        [HttpPost]
        public async Task<ApiResult> Add()
        {
            try
            {
                string content = Request.Form["content"];
                UserDTO userDTO = Auth.GetLoginUser();
                _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);
                await _whisperService.Create(content,userDTO.Account,userDTO.Username,value);
                return ApiResult.Success();
            }
            catch (Exception)
            {
                return ApiResult.Error(HttpStatusCode.FORBIDDEN, "not login");
            }
        }
        [HttpPost]
        [Route("page")]
        public ApiResult LoadWhisper([FromBody] WhisperCondition condition)
        {
            if (condition.LoginUser)
            {
                UserDTO userDTO = Auth.GetLoginUser();
                condition.Account = userDTO.Account;
            }
            List<WhisperDTO> whisperModels = _whisperService.SelectPage(condition.CurrentPage, condition.PageSize, condition);
            int total = _whisperService.SelectCount(condition);
            return ApiResult.Success(new { list = whisperModels, total = total });
        }
    }
}
