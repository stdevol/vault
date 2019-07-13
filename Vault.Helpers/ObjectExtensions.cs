using System;
using System.Threading.Tasks;

namespace Vault.Helpers
{
    public static class ObjectExtensions
    {
        public static TTarget Apply<TSource, TTarget>(this TSource source, Func<TSource, TTarget> func)
            => func(source);

        public static Task<T> AsTask<T>(this T source)
            => Task.FromResult(source);
    }
}
