using Blog.Sms.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Sms.Application.Service
{
   public interface ISingalrService
    {
       void SendWhisper(List<WhisperDTO> whisperDTOs);

        void SendTidingsCount(string account,int count);
    }
}
