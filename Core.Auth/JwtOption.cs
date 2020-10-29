using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Auth
{
   public class JwtOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireMinutes { get; set; }
        public string Secret { get; set; }
    }
}
