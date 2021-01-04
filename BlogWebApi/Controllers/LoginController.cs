using Blog.Application.DTO;
using Blog.Application.Service;
using Core.Cache;
using Core.Common;
using Core.Common.Http;
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
        public LoginController(IUserService userService, ICacheFactory cacheFactory)
        {
            _userService = userService;
            _cacheFactory = cacheFactory;
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
    }
}
