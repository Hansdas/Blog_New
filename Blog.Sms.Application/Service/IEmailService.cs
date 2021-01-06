using Blog.Sms.Application.EventHandler.EmailEventHandler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Sms.Application.Service
{
   public interface IEmailService
    {
        Task Send(EmailData emailInfo);
    }
}
