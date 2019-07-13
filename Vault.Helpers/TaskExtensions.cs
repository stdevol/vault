using System;
using System.Threading.Tasks;

namespace Vault.Helpers
{
    public static class TaskExtensions
    {
        public static async Task<TTo> Then<T, TTo>(this Task<T> task, Func<T, TTo> selector)
            => Guard
                .NotNull(selector, nameof(selector))
                .Invoke(await task);
    }
}
