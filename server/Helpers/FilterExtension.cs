using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace TodoApi.Helpers
{
    public static class IQueryableFilterExtension
    {
        public static IQueryable<object> RnDFilter(
            this IQueryable<object> dbSet,
            string? where = null,
            object[]? whereParams = null,
            string[]? includes = null,
            int take = 0,
            int skip = 0,
            string? orderBy = null
        )
        {
            var query = dbSet.AsNoTracking();

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

        public static IQueryable<object> RnDFilter(
            this IQueryable<object> dbSet,
            string? where = null,
            string? whereParams = null,
            string? includes = null,
            int take = 0,
            int skip = 0,
            string? orderBy = null
        )
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


        public static int RnDFilterCount(
            this IQueryable<object> dbSet,
            string? where = null,
            object[]? whereParams = null
        )
        {
            var query = dbSet.AsNoTracking();

            if (!string.IsNullOrEmpty(where))
                query = query.Where(where, whereParams);

            return query.Count();
        }

    }
}
