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
    [Route("api/gallery")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private IGalleryService _galleryService;
        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }
        [Route("upload")]
        [HttpPost]
        public ApiResult Upload([FromBody]GalleryDTO galleryDTO)
        {
            _galleryService.Upload(galleryDTO);
            return ApiResult.Success();
        }
        [Route("list/page")]
        [HttpPost]
        public ApiResult Upload([FromBody] GalleryCondition galleryCondition)
        {
            List<GalleryDTO> list = _galleryService.GetListPage(galleryCondition.CurrentPage, galleryCondition.PageSize, galleryCondition);
            int total = _galleryService.GetCount(galleryCondition);
            return ApiResult.Success(new { list,total});
        }
    }
}
