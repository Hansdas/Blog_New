
using Core.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class User:Entity<int>
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Username { get;  set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get;  set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get;  set; }
        /// <summary>
        /// 登录类型
        /// </summary>
        public LoginType? LoginType { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; } = Sex.男;
        /// <summary>
        /// 是否失效
        /// </summary>
        public bool IsValid { get;  set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get;  set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get;  set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDate { get;  set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Sign { get;  set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPhoto { get;  set; }
    }
}
