using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Socket.Singalr
{
  public  interface ISingalrContent
    {

        /// <summary>
        /// 获取connectionIds
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        List<string> GetConnectionIds(string value);
        /// <summary>
        ///设置connectionid与user的关系
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="value"></param>
        void SetConnectionMaps(string connectionId, string value);
        /// <summary>
        /// 删除connection关系
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
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
