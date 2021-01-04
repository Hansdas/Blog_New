using Blog.Sms.Application.DTO;
using Core.Socket;
using Core.Socket.Singalr;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogSmsApi.Controllers
{
    [Route("api/singlar")]
    [ApiController]
    public class SingalrController: ControllerBase
    {
        private ISingalrContent _singalrContent;
        public SingalrController(ISingalrContent singalrContent)
        {
            _singalrContent = singalrContent;
        }
        [HttpPost]
        [Route("whisper")]
        public async Task AddWhisper([FromBody]List<WhisperDTO> whisperDTOs)
        {
            Message message = new Message();
            message.Data = whisperDTOs;
           await _singalrContent.SendAllClientsMessage(message);
        }
    }
}
