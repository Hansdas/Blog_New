using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Socket.Singalr
{
    public class SingalrClient : Hub
    {
        private ISingalrContent _content;
        public SingalrClient(ISingalrContent content)
        {
            _content = content;
        }
        public void SetConnectionMaps(string account)
        {
            string connectionid = Context.ConnectionId;
            _content.SetConnectionMaps(connectionid, account);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _content.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
