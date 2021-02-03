using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Blog.Application.DTO;
using Blog.Domain;
using Blog.Repository;
using Core.EventBus;

namespace Blog.Application.Service.imp
{
    public class LeaveMessageService : ILeaveMessageService
    {
        private ILeaveMessageRepository _leaveMessageRepository;
        private IEventBus _eventBus;
        public LeaveMessageService(ILeaveMessageRepository leaveMessageRepository, IEventBus eventBus)
        {
            _leaveMessageRepository = leaveMessageRepository;
            _eventBus = eventBus;
        }

        public void Add(LeaveMessageDTO leaveMessageDTO)
        {
            LeaveMessage leaveMessage = new LeaveMessage();
            leaveMessage.IsAction = false;
            leaveMessage.IsFriendLink = leaveMessageDTO.IsFriendLink;
            leaveMessage.CreateTime = DateTime.Now;
            leaveMessage.ContractEmail = leaveMessageDTO.ContractEmail;
            leaveMessage.Content = leaveMessageDTO.Content;
            _leaveMessageRepository.Insert(leaveMessage);
            EmailData emailData = new EmailData();
            emailData.Body = leaveMessage.Content;
            emailData.Subject = "天天博客有个新留言";
            _eventBus.Publish(emailData);

        }

        public List<LeaveMessageDTO> GetLeaveList(int currentPage)
        {
            Expression<Func<LeaveMessage, object>> orderByTimeDesc = s => s.CreateTime;
            IEnumerable<LeaveMessage> leaveMessages = _leaveMessageRepository.SelectByPage(currentPage, 5, null, orderByTimeDesc);
            List<LeaveMessageDTO> leaveMessageDTOs = new List<LeaveMessageDTO>();
            foreach (var item in leaveMessages)
            {
                LeaveMessageDTO leaveMessageDTO = new LeaveMessageDTO();
                leaveMessageDTO.IsFriendLink = item.IsFriendLink;
                if (item.IsFriendLink)
                {
                    var arr = item.Content.Split(';');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (i == 0)
                            leaveMessageDTO.Content += string.Format("邮箱：{0}<br>", arr[i]);
                        else if (i == 1)
                            leaveMessageDTO.Content += string.Format("站点名称：{0}<br>", arr[i]);
                        else if (i == 2)
                            leaveMessageDTO.Content += string.Format("站点地址：{0}<br>", arr[i]);
                        else
                            leaveMessageDTO.Content += string.Format("站点图标Url：{0}<br>", arr[i]);
                    }
                }
                else
                    leaveMessageDTO.Content = item.Content;
                leaveMessageDTO.ContractEmail = item.ContractEmail;
                leaveMessageDTO.CreateTime = item.CreateTime.ToString("yyyy-MM-dd: hh:mm");
                leaveMessageDTOs.Add(leaveMessageDTO);
            }
            return leaveMessageDTOs;
        }
        public int SelectCount()
        {
            return _leaveMessageRepository.SelectCount();
        }
        public List<FriendLinkDTO> GetFriendLinks()
        {
            IEnumerable<LeaveMessage> leaveMessages = _leaveMessageRepository.Select(s => s.IsFriendLink == true);
            List<FriendLinkDTO> friendLinkDTOs = new List<FriendLinkDTO>();
            foreach (var item in leaveMessages)
            {
                FriendLinkDTO friendLinkDTO = new FriendLinkDTO();
                var arr = item.Content.Split(';');
                for (int i = 0; i < arr.Length; i++)
                {
                    if (i == 1)
                        friendLinkDTO.WebName = arr[i];
                    else if (i == 2)
                        friendLinkDTO.link = arr[i];
                    else
                        friendLinkDTO.Img = arr[i];
                }
                friendLinkDTOs.Add(friendLinkDTO);
            }
            return friendLinkDTOs;
        }
    }
}
