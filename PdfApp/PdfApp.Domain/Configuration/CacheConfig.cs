using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Configuration
{
    public partial class CacheConfig : IConfig
    {
        /// <summary>
        /// Gets or sets the default cache time in seconds
        /// </summary>
        public int DefaultCacheTime { get; set; } = 30 * 24 * 60 * 60; // 1 hour

        /// <summary>
        /// Gets or sets the short term cache time in seconds
        /// </summary>
        public int ShortTermCacheTime { get; set; } = 180; // 3 minutes

        /// <summary>
        /// Gets or sets the bundled files cache time in seconds
        /// </summary>
        public int BundledFilesCacheTime { get; set; } = 120;
    }
}
