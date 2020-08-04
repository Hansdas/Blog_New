using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Repoistory
{
   public interface IRepoistory<TEntity,T> where TEntity : Entity<T>, new()
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity SelectById(T id);
        /// <summary>
        /// 根据条件查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        TEntity SelectSingle(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>

        IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, bool>> orderBy=null);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IEnumerable<TEntity> SelectByPage(int currentPage, int pageSize,Expression<Func<TEntity, bool>> where=null, Expression<Func<TEntity, bool>> orderBy=null);
        /// <summary>
        /// 更改数据
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        void Delete(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        void DeleteById(T id);

    }
}
