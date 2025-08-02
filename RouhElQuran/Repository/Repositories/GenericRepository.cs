using Core.HelperModel.PaginationModel;
using Core.IRepo;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    //public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    //{
    //    private readonly RouhElQuranContext _dbContext;
    //    private readonly DbSet<TEntity> _dbSet;

    //    public GenericRepository(RouhElQuranContext dbContext)
    //    {
    //        _dbContext = dbContext;
    //        _dbSet = _dbContext.Set<TEntity>();

    //    }
    //    public IQueryable<TEntity> GetAllQueryableAsync()
    //    {
    //        var result = _dbSet;
    //        return result;

    //    }

    //    public async Task<TEntity> AddAsync(TEntity entity)
    //    {
    //        var result = await _dbSet.AddAsync(entity);
    //        await _dbContext.SaveChangesAsync();
    //        return result.Entity;
    //    }
    //    public async Task<TEntity> UpdateAsync(TEntity entity)
    //    {
    //        _dbSet.Update(entity);
    //        await _dbContext.SaveChangesAsync();
    //        return entity;
    //    }

    //    public async Task<TEntity?> GetByIdAsync(int? id)
    //    {
    //        return await _dbSet.FindAsync(id);
    //    }
    //    public async Task<TEntity> DeleteAsync(int? id)
    //    {
    //        var entity = await GetByIdAsync(id);
    //        if (entity != null)
    //        {
    //            _dbSet.Remove(entity);
    //            await _dbContext.SaveChangesAsync();
    //            return entity;
    //        }
    //        return null;
    //    }


    //    public async Task BeginTransactionAsync()
    //    {
    //        await _dbContext.Database.BeginTransactionAsync();
    //    }

    //    public async Task CommitTransactionAsync()
    //    {
    //        await _dbContext.Database.CommitTransactionAsync();
    //    }

    //    public async Task RollbackTransactionAsync()
    //    {
    //        await _dbContext.Database.RollbackTransactionAsync();
    //    }

    //    public Task<TEntity?> Get(Expression<Func<TEntity, bool>> where, bool asNoTracking = true)
    //    {
    //        IQueryable<TEntity> query = _dbSet.Where(where);

    //        if (asNoTracking)
    //            query = query.AsNoTracking();

    //        return query.FirstOrDefaultAsync();
    //    }


    //    public Task<TEntity?> Get(
    //        Expression<Func<TEntity, bool>> where,
    //        bool asNoTracking = true,
    //        params Expression<Func<TEntity, object>>[] includes)
    //    {
    //        IQueryable<TEntity> query = _dbSet;

    //        if (asNoTracking)
    //            query = query.AsNoTracking();

    //        if (where != null)
    //            query = query.Where(where);

    //        if (includes != null && includes.Length != 0)
    //        {
    //            foreach (var include in includes)
    //            {
    //                query = query.Include(include);
    //            }
    //        }

    //        return query.FirstOrDefaultAsync();
    //    }



    //    public async Task SaveAsync() =>
    //        await _dbContext.SaveChangesAsync();
    //}

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        internal DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<List<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }


        public virtual async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "", bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
                query = query.AsNoTracking();

            foreach (var include in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(include.Trim());

            return await query.FirstOrDefaultAsync(filter);
        }


        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(T entity) => _dbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
            => filter != null ? await _dbSet.CountAsync(filter) : await _dbSet.CountAsync();

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
            => await _dbSet.AnyAsync(filter);

        public virtual async Task<List<TResult>> SelectListAsync<TResult>(
            Expression<Func<T, bool>>? filter, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.Select(selector).ToListAsync();
        }
        public virtual async Task<TResult?> SelectFirstOrDefaultAsync<TResult>(
          Expression<Func<T, bool>>? filter, Expression<Func<T, TResult>> selector,
          params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.Select(selector).FirstOrDefaultAsync();
        }


        public virtual async Task<PaginationRequest<T>> GetPagedAsync(
            Expression<Func<T, bool>>? filter = null,
            int page = 1,
            int pageSize = 10,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(include.Trim());

            var total = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationRequest<T>
            {
                Items = items,
                TotalCount = total,
                CurrentPage = page,
                PageSize = pageSize,
            };


        }
    }
}