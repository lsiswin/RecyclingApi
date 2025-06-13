using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Domain.Entities.Data;

namespace RecyclingApi.Application.Repositories
{
    /// <summary>
    /// 通用仓储实现
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// 根据条件获取并包含关联实体
        /// </summary>
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.Where(predicate);
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// 根据ID获取实体，并包含关联实体
        /// </summary>
        public async Task<TEntity> GetByIdAsync(object id, params string[] includeProperties)
        {
            // 先查找实体
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return null;

            // 使用另一个查询来获取包含所有关联实体的完整实体
            var keyProperty = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties[0];
            var keyName = keyProperty.Name;
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, keyName);
            var constant = Expression.Constant(id);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

            IQueryable<TEntity> query = _dbSet.Where(lambda);
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        public async Task DeleteAsync(object id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        public async Task<PagedResponseDto<TEntity>> GetPagedListAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, object>> orderBy = null,
            bool isDesc = false)
        {
            return await GetPagedListAsync(pageIndex, pageSize, null, orderBy, isDesc);
        }

        /// <summary>
        /// 根据条件获取分页结果
        /// </summary>
        public async Task<PagedResponseDto<TEntity>> GetPagedListAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> orderBy = null,
            bool isDesc = false)
        {
            return await GetPagedListAsync(pageIndex, pageSize, predicate, orderBy, isDesc, null);
        }

        /// <summary>
        /// 根据条件获取分页结果，并包含关联实体
        /// </summary>
        public async Task<PagedResponseDto<TEntity>> GetPagedListAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> orderBy = null,
            bool isDesc = false,
            params string[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            // 应用查询条件
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // 包含关联实体
            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            // 获取总记录数
            var totalCount = await query.CountAsync();

            // 应用排序
            if (orderBy != null)
            {
                query = isDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }

            // 应用分页
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 计算总页数
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 构建分页响应
            return new PagedResponseDto<TEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
} 