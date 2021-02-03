using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Socket.Singalr
{
  public  interface ISingalrContent
    {

        /// <summary>
        /// 向所有客户端（用户）发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAllClientsMessage(Message message);
        /// <summary>
        /// 向指定的部分客户端（用户）发送消息
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendSomeClientsMessage(IReadOnlyList<string> connectionIds, Message message);
        /// <summary>
        /// 向指定的客户端（用户）发送消息
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendClientMessage(Message message);
    }
}
