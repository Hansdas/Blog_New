using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repoistory
{
   public interface IRepoistory<TEntity,T> where TEntity :class
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity SelectById(T id);  
    }
}
