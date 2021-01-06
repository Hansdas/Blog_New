using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Sms.Application.EventHandler.EmailEventHandler;
using Blog.Sms.Repository;
using Core.Common.Email;
using Core.Domain.Core;
using Newtonsoft.Json;

namespace Blog.Sms.Application.Service.Imp
{
    public class EmailService : IEmailService
    {
        private ISysConfigRepository _sysConfigRepository;
        private string sender = "www.ttblog.site";
        public EmailService(ISysConfigRepository sysConfigRepository)
        {
            _sysConfigRepository = sysConfigRepository;
        }
        public async Task Send(EmailData emailInfo)
        {
            string value= _sysConfigRepository.SelectValue(ConstantKey.MAIL_CONFIG_KEY);
            MailServerConfig mailServerConfig = JsonConvert.DeserializeObject<MailServerConfig>(value);
            MailBody mailBody = new MailBody();
            mailBody.Body = emailInfo.Body;
            mailBody.Revicer = !string.IsNullOrEmpty(emailInfo.Revicer)? emailInfo.Revicer: mailServerConfig.Account;
            mailBody.RevicerAddress = !string.IsNullOrEmpty(emailInfo.RevicerAddress) ? emailInfo.RevicerAddress : mailServerConfig.Account;
            mailBody.Sender = sender;
            mailBody.SenderAddress =mailServerConfig.Account;
            mailBody.Subject = emailInfo.Subject;
            Mail mail = new Mail();
            await mail.SendMail(mailBody,mailServerConfig);
        }
    }
}
