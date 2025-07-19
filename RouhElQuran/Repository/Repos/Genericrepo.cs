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

        public Genericrepo(RouhElQuranContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<TEntity> GetAllAsync()
        {
            var result =  _dbContext.Set<TEntity>();
            return result;

        }

        public IQueryable<TEntity> GetByIdAsync(int? id)
        {
            var result =  _dbContext.Set<TEntity>();
            return result;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
           var result = await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }


        public async Task<TEntity> DeleteAsync(int? id)
        {
            var entity = GetByIdAsync(id).FirstOrDefault();
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
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



        //public async Task<TEntity> GetLastElementByID(Expression<Func<TEntity, int>> expression)
        //{
        //    return await context.Set<TEntity>().OrderByDescending(expression).FirstOrDefaultAsync();
        //}



        //public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> expression)
        //{
        //    //if (typeof(TEntity) == typeof(CoursePlan))
        //    //{
        //    //    var Result = await context.Set<CoursePlan>().FirstOrDefaultAsync(expression as Expression<Func<CoursePlan, bool>>);
        //    //    return Result as TEntity;
        //    //}
        //    //else
        //    //{
        //    var result = await context.Set<TEntity>().FirstOrDefaultAsync(expression);
        //    return result;
        //    //}
        //}
    }
}