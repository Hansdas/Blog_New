using Blog.Domain.Tidings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Application.Condition
{
   public class TidingsCondition: PageCondition
    {
        /// <summary>
        /// 根据接收人账号查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool? IsRead { get; set; }

        public static Expression<Func<Tidings, bool>> BuildExpression(TidingsCondition condition)
        {
            Expression<Func<Tidings, bool>> expressLeft = null;
            if (!string.IsNullOrEmpty(condition.Account))
            {
                Expression<Func<Tidings, bool>> expressRight = s => s.ReviceUser == condition.Account;
                Expression.Add(expressLeft, expressRight);
            }
            if (condition.IsRead.HasValue)
            {
                Expression<Func<Tidings, bool>> expressRight = s => s.IsRead==condition.IsRead.Value;
                Expression.Add(expressLeft, expressRight);
            }
            return expressLeft;
        }
    }
}
