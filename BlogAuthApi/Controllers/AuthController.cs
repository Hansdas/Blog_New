using Core.Auth;
using Core.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogAuthApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController
    { 
        [Route("token")]
        [HttpGet]
        public ApiResult GetToken()
        {
            DateTime now = DateTime.Now;
            var claims = new Claim[]
                {
                    // 声明主题
                    new Claim(JwtRegisteredClaimNames.Sub, "Blog"),
                    //JWT ID 唯一标识符
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    // 发布时间戳 issued timestamp
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };
            JwtToken jwtToken = Jwt.GetToken(claims);
            return ApiResult.Success(jwtToken);
        }
    }
}
