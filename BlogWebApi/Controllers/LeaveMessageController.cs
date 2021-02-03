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
    [Route("api/leavemessage")]
    [ApiController]
    public class LeaveMessageController: ControllerBase
    {
        private ILeaveMessageService _leaveMessageService;
        public LeaveMessageController(ILeaveMessageService leaveMessageService)
        {
            _leaveMessageService = leaveMessageService;
        }
        [HttpGet]
        [Route("friendlinks")]
        public ApiResult GetFriendLinks()
        {
            List<FriendLinkDTO> friendLinks = _leaveMessageService.GetFriendLinks();
            return ApiResult.Success(friendLinks);
        }
        [Route("page/{currentPage}")]
        [HttpGet]
        public  ApiResult GetLeavaMessagePage(int currentPage)
        {
            List<LeaveMessageDTO> leaveMessageDTOs = _leaveMessageService.GetLeaveList(currentPage);
            int total = _leaveMessageService.SelectCount();
            return ApiResult.Success(new { list=leaveMessageDTOs,total=total});
        }
        [Route("add")]
        [HttpPost]
        public ApiResult Add([FromBody]LeaveMessageDTO leaveMessageDTO)
        {
            _leaveMessageService.Add(leaveMessageDTO);
            return ApiResult.Success();
        }
        [Route("add/friendlink")]
        [HttpPost]
        public ApiResult Add()
        {
            string email = Request.Form["email"];
            string siteName = Request.Form["siteName"];
            string siteUrl = Request.Form["siteUrl"];
            string siteImgUrl = Request.Form["siteImgUrl"];
            LeaveMessageDTO leaveMessageDTO = new LeaveMessageDTO();
            leaveMessageDTO.ContractEmail = email;
            leaveMessageDTO.Content = string.Format("{0};{1};{2};{3}", email, siteName, siteUrl, siteImgUrl);
            leaveMessageDTO.IsFriendLink = true;
            _leaveMessageService.Add(leaveMessageDTO);
            return ApiResult.Success();
        }
    }
}
