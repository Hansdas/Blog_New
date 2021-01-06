using Core.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application
{
    public class EmailData : EventData
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 发送者地址
        /// </summary>
        public string SenderAddress { get; set; }
        /// <summary>
        ///接收者
        /// </summary>
        public string Revicer { get; set; }
        /// <summary>
        /// 接收者地址
        /// </summary>
        public string RevicerAddress { get; set; }
        /// <summary>
        /// 正文类型
        /// </summary>
        public string BodyType { get; set; }
        /// <summary>
        /// 正文内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

    }
}
