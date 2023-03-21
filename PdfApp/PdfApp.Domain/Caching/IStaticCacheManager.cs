using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Caching
{
    public interface IStaticCacheManager : IDisposable
    {
        Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire);
        Task<T> GetAsync<T>(CacheKey key, Func<T> acquire);
        Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters);
        Task SetAsync(CacheKey key, object data);
        Task<bool> IsSetAsync(CacheKey key);
        Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters);
        Task ClearAsync();

        #region Cache key
        CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters);
        CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters);
        CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters);

        #endregion
    }
}
