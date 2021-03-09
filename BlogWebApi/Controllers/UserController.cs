using Blog.Application.Condition;
using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Cache;
using Core.Common.Http;
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
        private ITidingsService _tidingsService;
        public UserController(ITidingsService tidingsService)
        {
            _tidingsService = tidingsService;
        }
        [Route("loginuser")]
        [HttpGet]
        public ApiResult GetUser()
        {
            UserDTO userDTO = Auth.GetLoginUser();
            TidingsCondition tidingsCondition = new TidingsCondition();
            tidingsCondition.Account = userDTO.Account;
            tidingsCondition.IsRead = false;
            int count = _tidingsService.SelectCount(tidingsCondition); 
            return ApiResult.Success(new { photo = userDTO.HeadPhoto, count = count, account = userDTO.Account });
        }
        [Route("token")]
        [HttpGet]
        public ApiResult  GetUserByToken()
        {
            UserDTO userDTO= Auth.GetLoginUser();          
            return ApiResult.Success(userDTO);
        }
    }
}
