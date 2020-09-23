﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Http
{
  public class ApiResult
    {
        public ApiResult(string code, dynamic data)
        {
            Code = code;
            Data = data;
        }
        public ApiResult(string code, string msg)
        {
            Code = code;
            Msg = msg;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }
        public static ApiResult Success(dynamic data = null)
        {
            return new ApiResult(HttpStatusCode.SUCCESS, data);
        }
        public static ApiResult Error(string code, string msg)
        {
            return new ApiResult(code, msg);
        }
    }
}
