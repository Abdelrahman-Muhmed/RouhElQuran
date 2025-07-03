using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper.SortHelper
{
    public static class SortHelpercs
    {
        public static IQueryable<T> OrderByProperty<T>(
        this IQueryable<T> source,
        string propertyPath,
        bool IsDesc)
        {
            if (string.IsNullOrWhiteSpace(propertyPath))
                throw new ArgumentException("Property path must not be null or empty.", nameof(propertyPath));

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertyAccess = parameter;

            foreach (var propertyName in propertyPath.Split('.'))
            {
                propertyAccess = Expression.PropertyOrField(propertyAccess, propertyName);
            }

            var lambda = Expression.Lambda(propertyAccess, parameter);

            string methodName = IsDesc ? "OrderByDescending" : "OrderBy";

            var result = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), propertyAccess.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(result);
        }

        public static IOrderedEnumerable<IGrouping<TKey, TElement>> OrderGroupByProperty<TKey, TElement>(
          this IEnumerable<IGrouping<TKey, TElement>> source,
          string propertyPath,
          bool descending = false)
            {
                var param = Expression.Parameter(typeof(IGrouping<TKey, TElement>), "g");

                var firstCall = Expression.Call(typeof(Enumerable), "First", new[] { typeof(TElement) }, param);

                Expression body = firstCall;

                foreach (var prop in propertyPath.Split('.'))
                {
                    body = Expression.PropertyOrField(body, prop);
                }

                var lambda = Expression.Lambda<Func<IGrouping<TKey, TElement>, object>>(
                    Expression.Convert(body, typeof(object)), param);

                return descending
                    ? source.OrderByDescending(lambda.Compile())
                    : source.OrderBy(lambda.Compile());
            }



    }
}

