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
        public TidingsController(ITidingsService tidingsService)
        {
            _tidingsService = tidingsService;
        }
        private UserDTO GetLoginUser()
        {
            UserDTO userDTO = Auth.GetLoginUser();
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
