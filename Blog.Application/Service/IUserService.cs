using Blog.Application.DTO;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Application.Service
{
   public interface IUserService
    {
        /// <summary>
        /// 查询单个用户
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        UserDTO SelectSingle(Expression<Func<User, bool>> where);
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserDTO Login(string account,string password);
    }
}
