using PdfApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Caching
{
    public static partial class PdfAppEntityCacheDefaults<TEntity> where TEntity : BaseEntity
    {
        public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();
        public static CacheKey AllCacheKey => new CacheKey($"PfdApp.{EntityTypeName}.all.", AllPrefix, Prefix);
        public static string Prefix => $"PfdApp.{EntityTypeName}.";
        public static string AllPrefix => $"PfdApp.{EntityTypeName}.all.";
    }
}
