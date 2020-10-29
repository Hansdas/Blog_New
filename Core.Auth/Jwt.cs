using Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Auth
{
    public class Jwt
    {
        /// <summary>
        /// 返回jwt模型
        /// </summary>
        /// <returns></returns>
        public static JwtOption GetOption()
        {
            JwtOption option = ConfigureProvider.BuildModel<JwtOption>("jwtOption");
            return option;
        }
        /// <summary>
        ///  返回SymmetricSecurityKey
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            JwtOption option = GetOption();
            return GetSymmetricSecurityKey(option.Secret);
        }
        /// <summary>
        ///  返回SymmetricSecurityKey
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
        /// <summary>
        /// 返回token参数模型
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters GetTokenValidation()
        {
            JwtOption option = GetOption();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymmetricSecurityKey(option.Secret),
                ValidateIssuer = true,
                ValidIssuer = option.Issuer,
                ValidateAudience = true,
                ValidAudience = option.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };
            return tokenValidationParameters;
        }
        /// <summary>
        /// 获取jwt的token参数
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static JwtSecurityToken GetJwtParameters(Claim[] claims,JwtOption option=null)
        {
            if (option == null)
                option = GetOption();
            var jwt = new JwtSecurityToken(
               issuer: option.Issuer,
               audience: option.Audience,
               claims: claims,
               notBefore: DateTime.Now,
               expires: DateTime.Now.Add(TimeSpan.FromMinutes(option.ExpireMinutes)),
               signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(option.Secret), SecurityAlgorithms.HmacSha256)
            );
            return jwt;
        }
        /// <summary>
        /// 获取jwt
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static JwtToken GetToken(JwtSecurityToken tokenParameters)
        {
            JwtOption option = GetOption();
            string token=new JwtSecurityTokenHandler().WriteToken(tokenParameters);
            return new JwtToken(token, option.ExpireMinutes,DateTime.Now);
        }
        /// <summary>
        /// 获取jwt
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static JwtToken GetToken(Claim[] claims)
        {
            JwtOption option = GetOption();
            JwtSecurityToken jwtSecurityToken = GetJwtParameters(claims,option);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new JwtToken(token, option.ExpireMinutes, DateTime.Now);
        }
    }
}
