
using Core.CPlatform;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Log
{
   public static class ConfigureLog
    {
        public static void AddLog(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            LogOption logOption= ConfigureProvider.BuildModel<LogOption>("LogOption");
            LogUtils.EnableNlog = Convert.ToBoolean(logOption.EnableLog);
            loggerFactory.ConfigureNLog(logOption.ConfigPath);
        }
    }
}
