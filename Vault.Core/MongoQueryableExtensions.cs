using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Vault.Core
{
    public static class MongoQueryableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable)
            => queryable is IMongoQueryable<T> mq
                ? await IAsyncCursorSourceExtensions.ToListAsync(mq)
                : queryable.ToList();

        public static async Task<T[]> ToArrayAsync<T>(this IQueryable<T> queryable)
            => (await queryable.ToListAsync()).ToArray();
    }
}
