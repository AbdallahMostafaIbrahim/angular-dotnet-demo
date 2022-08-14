using Microsoft.EntityFrameworkCore;
using TodoApi.Helpers;

namespace TodoApi.Services
{
    public class CentralService<C> where C : DbContext
    {
        protected readonly C _dbContext;

        public CentralService(C dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Filter<T>(string? where = null, string? whereParams = null, string? includes = null, int take = 0, int skip = 0, string? orderBy = null) where T : class
        {
            return _dbContext.Set<T>().RnDFilter(
                where: where,
                whereParams: whereParams,
                includes: includes,
                take: take,
                skip: skip,
                orderBy: orderBy
            );
        }

    }
}
