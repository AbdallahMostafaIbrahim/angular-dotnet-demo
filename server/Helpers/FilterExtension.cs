using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace TodoApi.Helpers
{
    public static class IQueryableFilterExtension
    {
        public static IQueryable<dynamic> Set(this DbContext context, Type T)
        {
            MethodInfo method = typeof(DbContext).GetMethods().Where(x => x.Name == "Set").Where(x => x.IsGenericMethod).First();
            method = method.MakeGenericMethod(T);
            return method.Invoke(context, null) as IQueryable<dynamic>;
        }

        public static IQueryable<TEntity> RnDFilter<TEntity>(
            this IQueryable<TEntity> dbSet,
            string? where = null,
            object[]? whereParams = null,
            string[]? includes = null,
            int take = 0,
            int skip = 0,
            string? orderBy = null
        ) where TEntity : class
        {
            var query = dbSet;

            if (!string.IsNullOrEmpty(where))
                query = query.Where(where, whereParams);

            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderBy(orderBy);

            if (skip > 0)
                query = query.Skip(skip);

            if (take > 0)
                query = query.Take(take);

            if (includes != null)
                foreach (var inc in includes)
                    query = query.Include(inc);

            return query;
        }

        public static IQueryable<TEntity> RnDFilter<TEntity>(
            this IQueryable<TEntity> dbSet,
            string? where = null,
            string? whereParams = null,
            string? includes = null,
            int take = 0,
            int skip = 0,
            string? orderBy = null
        ) where TEntity : class
        {
            return dbSet.RnDFilter(
                where: where,
                whereParams: whereParams == null ? null : JsonConvert.DeserializeObject<object[]>(whereParams),
                includes: includes?.Split(','),
                take: take,
                skip: skip,
                orderBy: orderBy
            );
        }


        public static int RnDFilterCount<TEntity>(
            this IQueryable<TEntity> dbSet,
            string? where = null,
            object[]? whereParams = null
        ) where TEntity : class
        {
            var query = dbSet.AsNoTracking();

            if (!string.IsNullOrEmpty(where))
                query = query.Where(where, whereParams);

            return query.Count();
        }

    }
}
