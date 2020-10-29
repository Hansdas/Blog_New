using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Common.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi
{
    [Route("api/whisper")]
    [ApiController]
    public class WhisperController
    {
        private IWhisperService _whisperService;
        public WhisperController(IWhisperService whisperService)
        {
            _whisperService = whisperService;
        }
        [Route("square")]
        [HttpGet]
        public async Task<ApiResult> LoadSquare()
        {
          IList<WhisperDTO> whisperDTOs= await _whisperService.SelectPageCache(0,6);
            return ApiResult.Success(whisperDTOs);
        }
    }
}
