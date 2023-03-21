using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Configuration
{
    public partial class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public CacheConfig CacheConfig { get; set; } = new CacheConfig();
        public DeploymentConfig DeploymentConfig { get; set; } = new DeploymentConfig();
    }
}
