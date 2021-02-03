using Blog.Sms.Application.DTO;
using Core.Aop;
using Core.Socket;
using Core.Socket.Singalr;
using System.Collections.Generic;

namespace Blog.Sms.Application.Service.Imp
{
    public class SingalrService : ISingalrService, IInterceptorTag
    {
        private ISingalrContent _singalrContent;
        private readonly object _lock = new object();
        public SingalrService(ISingalrContent singalrContent)
        {
            _singalrContent = singalrContent;
        }
        public void SendWhisper(List<WhisperDTO> whisperDTOs)
        {
            throw new System.Exception("111");
            Message message = new Message();
            message.Data = whisperDTOs;
            _singalrContent.SendAllClientsMessage(message);
        }

        public void SendTidingsCount(string account, int count)
        {
            Message message = new Message();
            message.Data = count;
            message.Revicer = account;
            _singalrContent.SendClientMessage(message);
        }
    }
}
