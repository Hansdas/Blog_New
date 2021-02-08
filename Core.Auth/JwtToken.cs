using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Auth
{
   public class JwtToken
    {
        public string Token { get; set; }
        public int ExpireMinutes { get; set; }
        public string CreateTime { get; set; }
        public JwtToken(string token,int expireMinutes, DateTime createTime)
        {
            Token = token;
            ExpireMinutes = expireMinutes;
            CreateTime = createTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
