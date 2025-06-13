using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RecyclingApi.Application.DTOs;

namespace RecyclingApi.Application.Repositories
{
    /// <summary>
    /// 通用仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体集合</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体集合</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获取并包含关联实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="includeProperties">要包含的关联属性</param>
        /// <returns>实体集合</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体</returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// 根据ID获取实体，并包含关联实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <param name="includeProperties">要包含的关联属性</param>
        /// <returns>实体</returns>
        Task<TEntity> GetByIdAsync(object id, params string[] includeProperties);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Add(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id">实体ID</param>
        Task DeleteAsync(object id);

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<TEntity>> GetPagedListAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, object>> orderBy = null,
            bool isDesc = false);

        /// <summary>
        /// 根据条件获取分页结果
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<TEntity>> GetPagedListAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> orderBy = null,
            bool isDesc = false);

        /// <summary>
        /// 根据条件获取分页结果，并包含关联实体
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="isDesc">是否降序</param>
        /// <param name="includeProperties">要包含的关联属性</param>
        /// <returns>分页结果</returns>
        Task<PagedResponseDto<TEntity>> GetPagedListAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> orderBy = null,
            bool isDesc = false,
            params string[] includeProperties);

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>如果存在返回true，否则返回false</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
} 