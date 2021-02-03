using Blog.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Service
{
   public interface ILeaveMessageService
    {
        /// <summary>
        /// 提交留言
        /// </summary>
        void Add(LeaveMessageDTO leaveMessageDTO);
        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <returns></returns>
        List<LeaveMessageDTO> GetLeaveList(int currentPage);
        /// <summary>
        /// 获取友链
        /// </summary>
        /// <returns></returns>
        List<FriendLinkDTO> GetFriendLinks();
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <returns></returns>
        int SelectCount();
    }
}
