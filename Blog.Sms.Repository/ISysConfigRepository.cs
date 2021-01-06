using Blog.Sms.Domain;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Sms.Repository
{
   public interface ISysConfigRepository : IRepository<SysConfig, int>
    {
        /// <summary>
        /// 查询配置的value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string SelectValue(string key);
    }
}
