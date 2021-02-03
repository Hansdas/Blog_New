using Blog.Application.DTO;
using Blog.Domain;
using Core.Cache;
using Core.Common;
using Core.CPlatform;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogWebApi
{
    public class Auth
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public const string SecurityKey = "BlogH123456789012";
        public const string issuer = "BlogAPI";
        public const string audience = "BlogUI";
        public class TokenModel
        {
            public string token { get; set; }
            public DateTime ExpireTime { get; set; }
        }
        public static string CreateToken(IEnumerable<Claim> claims)
        {
            ICacheFactory cacheFactory = ServiceLocator.Get<ICacheFactory>();
            ICacheClient cacheClient = cacheFactory.CreateClient(CacheType.Redis);
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecurityKey));
            var expires = DateTime.Now.AddMinutes(30);
            var token = new JwtSecurityToken(
                        issuer: issuer,
                        audience: audience,
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            TokenModel tokenModel = new TokenModel();
            tokenModel.ExpireTime = DateTime.Now.AddDays(7);
            tokenModel.token = jwtToken;
            cacheClient.StringSet(jwtToken, tokenModel, TimeSpan.FromDays(7));
            return jwtToken;
        }
        public static void RemoveLoginToken()
        {
            IHttpContextAccessor httpContext = ServiceLocator.Get<IHttpContextAccessor>();
            bool flag = httpContext.HttpContext.Request.Headers.TryGetValue("loginToken", out StringValues token);
            if (!flag)
                throw new AuthException();
            ICacheFactory cacheFactory = ServiceLocator.Get<ICacheFactory>();
            ICacheClient cacheClient = cacheFactory.CreateClient(CacheType.Redis);
            cacheClient.Remove(token);
        }
       
        public static UserDTO GetLoginUser()
        {
            ICacheFactory cacheFactory = ServiceLocator.Get<ICacheFactory>();
            ICacheClient cacheClient = cacheFactory.CreateClient(CacheType.Redis);
            IHttpContextAccessor  httpContext = ServiceLocator.Get<IHttpContextAccessor>();
            bool flag = httpContext.HttpContext.Request.Headers.TryGetValue("loginToken", out StringValues token);
            if (!flag)
                throw new AuthException();
            if(string.IsNullOrEmpty(token[0])||token[0]=="null")
                throw new AuthException();
            string value = cacheClient.StringGet(token);
            #region 判断是否过期
            TokenModel tokenModel=JsonConvert.DeserializeObject<TokenModel>(value);
            if(tokenModel.ExpireTime<DateTime.Now)
            {
                cacheClient.Remove(token);
                throw new AuthException();
            }
            if(DateTime.Now.AddDays(1)==tokenModel.ExpireTime)
            {
                tokenModel.ExpireTime = DateTime.Now.AddDays(7);
                cacheClient.StringSet(token, tokenModel, TimeSpan.FromDays(7));
            }
            #endregion
            if (string.IsNullOrEmpty(value))
                throw new AuthException();
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var json = decoder.Decode(token);
            return JsonConvert.DeserializeObject<UserDTO>(json);

        }
    }
}
