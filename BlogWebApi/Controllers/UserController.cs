using Blog.Application.DTO;
using Core.Cache;
using Core.Common.Http;
using Core.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private ICacheFactory _cacheFactory;
        private IHttpContextAccessor _httpContext;
        public UserController(ICacheFactory cacheFactory, IHttpContextAccessor httpContext)
        {
            _cacheFactory = cacheFactory;
            _httpContext = httpContext;
        }
        [Route("loginuser")]
        [HttpGet]
        public async Task<ApiResult> GetUser()
        {
            Auth auth = new Auth(_cacheFactory, _httpContext);
            UserDTO userDTO = auth.GetLoginUser();
            string url = ConstantKey.GATEWAY_HOST + "/sms/tidings/" + userDTO.Account;
            _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", value.ToString());
            string response = await httpClient.GetStringAsync(url);
            ApiResult result = JsonConvert.DeserializeObject<ApiResult>(response);
            int count = (int)result.Data;
            return ApiResult.Success(new { photo = userDTO.HeadPhoto, count = count, account = userDTO.Account });
        }
        [Route("token")]
        [HttpGet]
        public ApiResult  GetUserByToken()
        {
            Auth auth = new Auth(_cacheFactory,_httpContext);
            UserDTO userDTO= auth.GetLoginUser();          
            return ApiResult.Success(userDTO);
        }
    }
}
