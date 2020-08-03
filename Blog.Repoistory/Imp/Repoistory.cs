using Blog.Repoistory.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repoistory.Imp
{
    public abstract class Repoistory<TEntity, T> : IRepoistory<TEntity, T> where TEntity:class
    {
        protected DBContext dbContext;
        public Repoistory(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity SelectById(T id)
        {
            TEntity entity=  dbContext.Set<TEntity>().Find(id);

            return entity;
        }
    }
}
