using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Repository;

namespace Blog.Repository
{
   public interface IUserRepository: IRepository<User, int>
    {
        /// <summary>
        /// 根据账号查询名字与账号集合
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Dictionary<string, string> AccountWithName(IEnumerable<string> accounts);
    }
}
