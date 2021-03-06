﻿using Core.CPlatform.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.Repository.Imp
{
    public abstract class Repository<TEntity, T> : IRepository<TEntity, T> where TEntity : Entity<T>, new()
    {
        protected DbContext _dbContext;
        /// <summary>
        /// 是否降序
        /// </summary>
        protected bool desc=true;
        /// <summary>
        /// 默认时间排序
        /// </summary>
        private Expression<Func<TEntity, object>> defaultOrderBy = s => s.CreateTime;
        public Repository(DbContext dbContext)
        {
            desc = true;
            _dbContext = dbContext;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            TEntity result = _dbContext.Set<TEntity>().Add(entity).Entity;
            _dbContext.SaveChanges();
            return result;
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity SelectById(T id)
        {
            TEntity entity = _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(s => s.Id.Equals(id));
            return entity;
        }
        /// <summary>
        /// 根据条件查询单条数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity SelectSingle(Expression<Func<TEntity, bool>> where)
        {
            TEntity entity = AsQueryable(where).FirstOrDefault();
            return entity;
        }
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int SelectCount(Expression<Func<TEntity, bool>> where = null)
        {

            return AsQueryable(where).Count();
        }
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> orderBy = null)
        {
            return AsQueryable(where, orderBy);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> SelectByPage(int currentPage, int pageSize, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> orderBy = null)
        {
            int pageId = (currentPage - 1) * pageSize;
            IEnumerable<TEntity> result = AsQueryable(where, orderBy).Skip(pageId).Take(pageSize);
            return result;
        }
        protected IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> orderBy = null)
        {
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>().AsNoTracking();
            if (where != null)
                queryable = queryable.Where(where);
            orderBy = orderBy == null ? defaultOrderBy : orderBy;
            queryable = desc == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
            return queryable;
        }
        /// <summary>
        /// 更改数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            _dbContext.Entry<TEntity>(entity).Property(s => s.Id).IsModified = false;
            _dbContext.SaveChanges();

        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> queryable = AsQueryable(where);
            _dbContext.RemoveRange(queryable);
            _dbContext.SaveChanges();
        }

        public void DeleteById(T id)
        {
            TEntity entity = new TEntity() { Id = id };
            _dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }
    }
}
