using Core.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.DTO
{
   public class LeaveMessageDTO
    {
        public bool IsFriendLink { get; set; }
        public string Content { get; set; }
        public string ContractEmail { get; set; }
        public string CreateTime { get; set; }
    }
 
}
