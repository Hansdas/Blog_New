using Blog.Application.DTO;
using Blog.Domain;
using Core.Cache;
using Core.Common;
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
        private  IHttpContextAccessor _httpContext;
        private ICacheClient _cacheClient;
        public Auth(ICacheFactory cacheFactory)
        {
            _cacheClient = cacheFactory.CreateClient(CacheType.Redis);
        }
        public Auth(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }
        public Auth(ICacheFactory cacheFactory, IHttpContextAccessor httpContextAccessor)
        {
            _cacheClient = cacheFactory.CreateClient(CacheType.Redis);
            _httpContext = httpContextAccessor;
        }
        public UserDTO GetLoginUser()
        {
            return JsonConvert.DeserializeObject<UserDTO>(ResolveToken());
        }
        public string ResolveToken()
        {
            bool flag = _httpContext.HttpContext.Request.Headers.TryGetValue("loginToken", out StringValues token);
            if (!flag)
                throw new AuthException("not login");
            string value = _cacheClient.StringGet(token);
            if (string.IsNullOrEmpty(value))
                throw new AuthException("not login");
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var json = decoder.Decode(token);
            return json;
        }
        public string CreateToken(IEnumerable<Claim> claims)
        {
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
            _cacheClient.StringSet(jwtToken, tokenModel, TimeSpan.FromDays(7));
            return jwtToken;
        }
        public void RemoveLoginToken()
        {
            bool flag = _httpContext.HttpContext.Request.Headers.TryGetValue("loginToken", out StringValues token);
            if (!flag)
                throw new AuthException("not login");
            _cacheClient.Remove(token);
        }
    }
}
