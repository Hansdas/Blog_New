using Blog.Sms.Application.Service;
using Core.EventBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Sms.Application.EventHandler.EmailEventHandler
{
    public class CreateEmailHandler : IEventHandler
    {
        private IEmailService _emailService;
        public CreateEmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handler(EventData eventData)
        {
            if (eventData.GetType() != typeof(EmailData))
                throw new ArgumentException("EventData无法转换为EmailData");
            await _emailService.Send((EmailData)eventData);
        }
    }
}
