using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllAsync();

        Task<TEntity?> GetByIdAsync(int? id);
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> DeleteAsync(int? id);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<TEntity?> Get(Expression<Func<TEntity, bool>> where, bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity?> Get(Expression<Func<TEntity, bool>> where, bool asNoTracking = true);

        //bool IsExists(int id);
        //IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria,
        //    Expression<Func<TEntity, object>> orderBy = null, string[] includes = null);
        //Task<TEntity> Find(Expression<Func<TEntity, bool>> criteria,
        //    string[] includes = null);
        ///
        //Task<TEntity> attach(TEntity obj);
        //int count();
        //int count(Expression<Func<TEntity, bool>> expression);
    }
}