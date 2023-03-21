using PdfApp.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Caching
{
    public partial class CacheKey
    {
        #region Ctor

        public CacheKey(string key, params string[] prefixes)
        {
            Key = key;
            Prefixes.AddRange(prefixes.Where(prefix => !string.IsNullOrEmpty(prefix)));
        }

        #endregion

        public virtual CacheKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
        {
            var cacheKey = new CacheKey(Key, Prefixes.ToArray());

            if (!keyObjects.Any())
                return cacheKey;

            cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

            for (var i = 0; i < cacheKey.Prefixes.Count; i++)
                cacheKey.Prefixes[i] = string.Format(cacheKey.Prefixes[i], keyObjects.Select(createCacheKeyParameters).ToArray());

            return cacheKey;
        }

        #region Properties
        /// <summary>
        /// Gets or sets a cache key
        /// </summary>
        public string Key { get; protected set; }

        /// <summary>
        /// Gets or sets prefixes for remove by prefix functionality
        /// </summary>
        public List<string> Prefixes { get; protected set; } = new List<string>();

        /// <summary>
        /// Gets or sets a cache time in seconds
        /// </summary>
        public int CacheTime { get; set; } = Singleton<AppSettings>.Instance.CacheConfig.DefaultCacheTime;
        #endregion
    }
}
