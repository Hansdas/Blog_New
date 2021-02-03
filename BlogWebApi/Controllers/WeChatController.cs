using Core.Common;
using Core.Common.Http;
using Core.CPlatform;
using Core.Log;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Controllers
{

    [Route("api/wechat")]
    [ApiController]
    public class WeChatController: ControllerBase
    {
        private string Token = ConfigureProvider.configuration.GetSection("WeChatToken").Value;
        [Route("check")]
        [HttpGet]
        public ActionResult CheckSignature(string echoStr, string signature, string timestamp, string nonce)
        {
            string[] array = { Token, timestamp, nonce };
            Array.Sort(array);
            var tempStr = String.Join("", array);
            tempStr= EncrypUtil.Get_SHA1(tempStr);
            LogUtils.LogInfo("WeChatController", string.Format("echoStr：{0}；signature：{1}；timestamp：{2}；nonce：{3}；tempStr；{4}；token；{5}", echoStr, signature, timestamp, nonce, tempStr, Token));
            if (tempStr.Equals(signature))
               return Content(echoStr);
            else
                return Content("false");
        }
    }
}
