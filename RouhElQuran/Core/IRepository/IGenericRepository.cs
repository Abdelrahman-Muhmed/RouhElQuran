using Core.HelperModel.PaginationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool disableTracking = true,
            params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "", bool disableTracking = true);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);

        Task<List<TResult>> SelectListAsync<TResult>(
                    Expression<Func<T, bool>>? filter, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includes);

        Task<TResult?> SelectFirstOrDefaultAsync<TResult>(
          Expression<Func<T, bool>>? filter, Expression<Func<T, TResult>> selector,
          params Expression<Func<T, object>>[] includes);

        Task<PaginationRequest<T>> GetPagedAsync(Expression<Func<T, bool>>? filter = null, int page = 1, int pageSize = 10,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");
    }
}