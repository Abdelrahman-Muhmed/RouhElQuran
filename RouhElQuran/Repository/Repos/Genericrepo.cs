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
    public class Genericrepo<TEntity> : IGenericrepo<TEntity> where TEntity : class
    {
        private readonly RouhElQuranContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Genericrepo(RouhElQuranContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();

        }
        public IQueryable<TEntity> GetAllAsync()
        {
            var result = _dbSet;
            return result;

        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> GetByIdAsync(int? id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<TEntity> DeleteAsync(int? id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            return null;
        }


        public async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        public Task<TEntity?> Get(Expression<Func<TEntity, bool>> where, bool asNoTracking = true)
        {
            IQueryable<TEntity> query = _dbSet.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefaultAsync();
        }


        public Task<TEntity?> Get(
            Expression<Func<TEntity, bool>> where,
            bool asNoTracking = true,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (where != null)
                query = query.Where(where);

            if (includes != null && includes.Length != 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefaultAsync();
        }



        public async Task SaveAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}