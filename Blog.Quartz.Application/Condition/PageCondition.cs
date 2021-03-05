using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Quartz.Application.Condition
{
    public class PageCondition
    {
        /// <summary>
        /// 分页起始页，从1开始
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageSize { get; set; }

        #region 将layui分页参数替换
        /// <summary>
        /// 分页起始页，从1开始
        /// </summary>
        public int Limit
        {
            set
            {
                PageSize = value;
            }
        }
        /// <summary>
        /// 分页数量
        /// </summary>
        public int Page
        {
            set
            {
                CurrentPage = value;
            }
        }
        #endregion
    }
}
