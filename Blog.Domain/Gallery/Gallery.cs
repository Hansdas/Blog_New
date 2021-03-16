using Core.CPlatform.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Gallery
{
    /// <summary>
    /// 图库
    /// </summary>
   public class Gallery: Entity<int>
    {
        /// <summary>
        /// 上传者
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Lable { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
