using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Socket
{
    public class Message
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public dynamic Data { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Revicer { get; set; }
        /// <summary>
        /// 接收人集合
        /// </summary>
        public List<string> Revicers { get; set; }
    }
}
