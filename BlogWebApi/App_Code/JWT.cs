using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi
{
    public class JWT
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public const string SecurityKey = "BlogH123456789012";
        public const string issuer = "BlogWebApi";
        public const string audience = "BlogUI";
    }
}
