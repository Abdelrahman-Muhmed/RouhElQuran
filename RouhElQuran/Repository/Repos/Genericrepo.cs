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
        private readonly RouhElQuranContext context;

        public Genericrepo(RouhElQuranContext context)
        {
            this.context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Instructor))
                return (IEnumerable<TEntity>)await context.Set<Instructor>().Include(e => e.User_id).ToListAsync();
            var r = await context.Set<TEntity>().ToListAsync();
            return r;

		}

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var res = await context.Set<TEntity>().FindAsync(id);
            return res;
        }

        public async Task<TEntity> GetLastElementByID(Expression<Func<TEntity, int>> expression)
        {
            return await context.Set<TEntity>().OrderByDescending(expression).FirstOrDefaultAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }

        public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> expression)
        {
            //if (typeof(TEntity) == typeof(CoursePlan))
            //{
            //    var Result = await context.Set<CoursePlan>().FirstOrDefaultAsync(expression as Expression<Func<CoursePlan, bool>>);
            //    return Result as TEntity;
            //}
            //else
            //{
            var result = await context.Set<TEntity>().FirstOrDefaultAsync(expression);
            return result;
            //}
        }
    }
}