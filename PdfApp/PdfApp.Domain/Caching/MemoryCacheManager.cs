﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using PdfApp.Domain.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Caching
{
    public partial class MemoryCacheManager : CacheKeyService, IStaticCacheManager
    {
        #region Fields

        // Flag: Has Dispose already been called?
        private bool _disposed;

        private readonly IMemoryCache _memoryCache;

        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes = new ConcurrentDictionary<string, CancellationTokenSource>();
        private static CancellationTokenSource _clearToken = new CancellationTokenSource();

        #endregion

        #region Ctor

        public MemoryCacheManager(IOptions<AppSettings> appSettings, IMemoryCache memoryCache) : base(appSettings)
        {
            _memoryCache = memoryCache;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        private bool IsSet(CacheKey key)
        {
            return _memoryCache.TryGetValue(key.Key, out _);
        }

        /// <summary>
        /// Prepare cache entry options for the passed key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cache entry options</returns>
        private MemoryCacheEntryOptions PrepareEntryOptions(CacheKey key)
        {
            //set expiration time for the passed cache key
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(key.CacheTime)
            };

            //add tokens to clear cache entries
            options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
            foreach (var keyPrefix in key.Prefixes.ToList())
            {
                var tokenSource = _prefixes.GetOrAdd(keyPrefix, new CancellationTokenSource());
                options.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
            }

            return options;
        }

        /// <summary>
        /// Remove the value with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        public void Remove(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            cacheKey = PrepareKey(cacheKey, cacheKeyParameters);
            _memoryCache.Remove(cacheKey.Key);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove the value with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        public Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            Remove(cacheKey, cacheKeyParameters);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
        {
            if ((key?.CacheTime ?? 0) <= 0)
                return await acquire();

            if (_memoryCache.TryGetValue(key.Key, out T result))
                return result;

            result = await acquire();

            if (result != null)
                await SetAsync(key, result);

            return result;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
        {
            if ((key?.CacheTime ?? 0) <= 0)
                return acquire();

            var result = _memoryCache.GetOrCreate(key.Key, entry =>
            {
                entry.SetOptions(PrepareEntryOptions(key));

                return acquire();
            });

            //do not cache null value
            if (result == null)
                await RemoveAsync(key);

            return result;
        }

        /// <summary>
        /// Add the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        public Task SetAsync(CacheKey key, object data)
        {
            if ((key?.CacheTime ?? 0) <= 0 || data == null)
                return Task.CompletedTask;

            _memoryCache.Set(key.Key, data, PrepareEntryOptions(key));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        public Task<bool> IsSetAsync(CacheKey key)
        {
            return Task.FromResult(IsSet(key));
        }

        /// <summary>
        /// Remove items by cache key prefix
        /// </summary>
        /// <param name="prefix">Cache key prefix</param>
        /// <param name="prefixParameters">Parameters to create cache key prefix</param>
        public Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            prefix = PrepareKeyPrefix(prefix, prefixParameters);

            _prefixes.TryRemove(prefix, out var tokenSource);
            tokenSource?.Cancel();
            tokenSource?.Dispose();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public Task ClearAsync()
        {
            _clearToken.Cancel();
            _clearToken.Dispose();

            _clearToken = new CancellationTokenSource();

            foreach (var prefix in _prefixes.Keys.ToList())
            {
                _prefixes.TryRemove(prefix, out var tokenSource);
                tokenSource?.Dispose();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _memoryCache.Dispose();

            _disposed = true;
        }
        #endregion
    }
}
