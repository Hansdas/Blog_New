using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Auth.IdentityServer4
{
   public class IdentityOption
    {
        /// <summary>
        /// 认证服务器地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// 是否使用https
        /// </summary>
        public bool UseHttps { get; set; }
        /// <summary>
        /// 秘钥Secret
        /// </summary>
        public string Secret { get; set; }
    }
}
