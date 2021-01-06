using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Cache;
using Core.Common;
using Core.Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;
        private ICacheFactory _cacheFactory;
        private IHttpContextAccessor _httpContext;
        public LoginController(IUserService userService, ICacheFactory cacheFactory,  IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _cacheFactory = cacheFactory;
            _httpContext = httpContext;
    }
        [Route("login")]
        [HttpPost]
        public ApiResult Login()
        {
            string account = Request.Form["account"];
            string password = Request.Form["password"];
            try
            {
                UserDTO user = _userService.Login(account, password);
                IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", user.Account),
                    new Claim("username", user.Username),
                    new Claim("sex", user.Sex),
                    new Claim("birthDate", user.BirthDate),
                    new Claim("email",user.Email),
                    new Claim("sign",user.Sign),
                    new Claim("phone",user.Phone),
                    new Claim("headPhoto",user.HeadPhoto)
                };
                string token = new Auth(_cacheFactory).CreateToken(claims);
                return ApiResult.Success(token);
            }
            catch (AuthException ex)
            {
                return ApiResult.Error("403", ex.Message);
            }
        }
        [Route("login/out")]
        [HttpGet]
        public ApiResult LoginOut()
        {
            try
            {
                Auth auth = new Auth(_cacheFactory, _httpContext);
                auth.RemoveLoginToken();
                return ApiResult.Success();
            }
            catch (AuthException ex)
            {
                return ApiResult.Error("403", ex.Message);
            }
        }
        [Route("login/logon")]
        [HttpPost]
        public ApiResult LoginOn([FromBody]UserDTO userDTO)
        {
            try
            {
                _userService.Create(userDTO);
                return ApiResult.Success();
            }
            catch (AuthException ex)
            {
                return ApiResult.Error("400", ex.Message);
            }
        }
        [Route("login/qq/{code}")]
        [HttpGet]
        public async Task<ApiResult> QQLogin(string code)
        {
            string accessToken = await QQClient.GetAccessToken(code);
            string openId = await QQClient.GetOpenId(accessToken);
            UserDTO userDTO = await QQClient.GetQQUser(accessToken, openId);
            _userService.CreateQQUser(userDTO);
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", userDTO.Account),
                    new Claim("username", userDTO.Username),
                    new Claim("sex", userDTO.Sex),
                    new Claim("birthDate",string.IsNullOrEmpty(userDTO.BirthDate)?"":userDTO.BirthDate),
                    new Claim("email", string.IsNullOrEmpty(userDTO.Email)?"":userDTO.Email),
                    new Claim("sign", string.IsNullOrEmpty(userDTO.Sign)?"":userDTO.Sign),
                    new Claim("phone",string.IsNullOrEmpty(userDTO.Phone)?"":userDTO.Phone),
                    new Claim("headPhoto", string.IsNullOrEmpty(userDTO.HeadPhoto)?"":userDTO.HeadPhoto)
                };
            string jwtToken = new Auth(_cacheFactory).CreateToken(claims);
            return ApiResult.Success(jwtToken);
        }
    }
}
