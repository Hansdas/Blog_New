using Blog.Sms.Application.DTO;
using Blog.Sms.Application.Service;
using Core.Common.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BlogSmsApi.Controllers
{
    [Route("api/singlar")]
    [ApiController]
    public class SingalrController : ControllerBase
    {
        private ISingalrService _singalrService;
        public SingalrController(ISingalrService singalrService)
        {
            _singalrService = singalrService;
        }
        [HttpPost]
        [Route("whisper")]
        public ApiResult AddWhisper([FromBody]List<WhisperDTO> whisperDTOs)
        {
            _singalrService.SendWhisper(whisperDTOs);
            return ApiResult.Success();
        }
        [HttpPost]
        [Route("tidings/count")]
        public ApiResult GetTidingsCount()
        {
            string userAccount = Request.Form["user"];
            int count = Convert.ToInt32(Request.Form["count"]);
            _singalrService.SendTidingsCount(userAccount, count);
            return ApiResult.Success();
        }
    }
}
