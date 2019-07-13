using System;
using System.Linq;
using System.Linq.Expressions;

namespace Vault.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> filter)
        {
            Guard.NotNull(query, nameof(query));
            if (!condition)
                return query;

            Guard.NotNull(filter, nameof(filter));
            return query.Where(filter);
        }
    }
}
