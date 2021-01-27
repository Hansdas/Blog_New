using Core.Auth;
using Core.Common.EnumExtensions;
using Core.Common.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogAuthApi
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private IHttpClientFactory _httpClientFactory;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        //[Route("token")]
        //[HttpGet]
        //public ApiResult GetToken()
        //{
        //    DateTime now = DateTime.Now;
        //    var claims = new Claim[]
        //        {
        //            // 声明主题
        //            new Claim(JwtRegisteredClaimNames.Sub, "Blog"),
        //            //JWT ID 唯一标识符
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            // 发布时间戳 issued timestamp
        //            new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
        //        };
        //    JwtToken jwtToken = Jwt.GetToken(claims);
        //    return ApiResult.Success(jwtToken);
        //}

        [Route("credentials/token")]
        [HttpGet]
        public async Task<ApiResult> GetToken()
        {
            Request.Headers.TryGetValue("client_id", out StringValues clientId);
            Request.Headers.TryGetValue("client_secret", out StringValues clientSecret);
            HttpClient httpClient= _httpClientFactory.CreateClient();
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("client_id", clientId);
            headers.Add("client_secret", clientSecret);
            headers.Add("grant_type", "client_credentials");
            FormUrlEncodedContent content = new FormUrlEncodedContent(headers);
            var response=await httpClient.PostAsync("http://localhost:5003/connect/token",content);
            if (!response.IsSuccessStatusCode)
                return ApiResult.Error(HttpStatusCode.BAD_REQUEST, response.StatusCode.GetEnumText());
            var responseString=await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(responseString);
            int expires = result.expires_in;
            int minutes = new TimeSpan(0, 0, expires).Minutes;
            string token = result.access_token;
            JwtToken jwtToken = new JwtToken(token, minutes, DateTime.Now);
            return ApiResult.Success(jwtToken);          
        }
    }
}
