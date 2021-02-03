using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Socket.Singalr
{
    public class SingalrContent : ISingalrContent
    {
       
        private IHubContext<SingalrClient> _hubContext;
        public SingalrContent(IHubContext<SingalrClient> hubContext)
        {
            _hubContext = hubContext;
        }
          
        #region 向客户端发送消息
        public async Task SendAllClientsMessage(Message message)
        {
            await _hubContext.Clients.All.SendAsync("AllReviceMesage", message);
        }
        public async Task SendSomeClientsMessage(IReadOnlyList<string> connectionIds, Message message)
        {
            if (connectionIds == null || connectionIds.Count == 0)
                throw new ArgumentNullException("指定的客户端连接为空");
            await _hubContext.Clients.Clients(connectionIds).SendAsync("SendClientMessage", message);
        }

        public async Task SendClientMessage(Message message)
        {
            if (string.IsNullOrEmpty(message.Revicer))
                throw new ArgumentNullException("指定的客户端连接为空");
            IReadOnlyList<string> connectionsByUser = SingalrConnection.GetConnectionIds(message.Revicer);
            await _hubContext.Clients.Clients(connectionsByUser).SendAsync("ReviceMesage", message);
        }
        #endregion
    }
}
