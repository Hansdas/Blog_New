using Blog.Domain.Tidings;
using Core.Repository.Where;
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
            Type targetType = typeof(Tidings);
            ParseCondition<Tidings> parse = new ParseCondition<Tidings>(targetType);
            if (!string.IsNullOrEmpty(condition.Account))
                parse.BuildExpression("ReviceUser", condition.Account,ConditionOperation.Equal);
            if (condition.IsRead.HasValue)
                parse.BuildExpression("IsRead", condition.IsRead.Value.ToString(), ConditionOperation.Equal);
            return parse.BuildWhereExpression(); ;
        }
    }
}
