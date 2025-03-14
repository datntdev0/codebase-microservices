using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace datntdev.Abp.Linq
{
    public class NullAsyncQueryableExecuter : IAsyncQueryableExecuter
    {
        public static NullAsyncQueryableExecuter Instance { get; } = new NullAsyncQueryableExecuter();

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(queryable.Count());
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(queryable.ToList());
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(queryable.Any());
        }
    }
}
