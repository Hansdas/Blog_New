using Core.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Filter
{
    /// <summary>
    /// 全局异常拦截器
    /// </summary>
   public class GlobaExceptionFilterAttribute: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string message = context.Exception.Message;
            ApiResult apiResult = ApiResult.Error(HttpStatusCode.ERROR, message);
            context.Result = new JsonResult(apiResult);
            context.ExceptionHandled = true;
        }
    }
}
