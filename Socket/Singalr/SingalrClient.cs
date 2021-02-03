using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Socket.Singalr
{
    public class SingalrClient : Hub
    {
        public void SetConnectionMaps(string account)
        {
            string connectionid = Context.ConnectionId;
            SingalrConnection.SetConnectionMaps(connectionid, account);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            SingalrConnection.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
