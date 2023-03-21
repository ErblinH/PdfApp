using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Configuration
{
    public class DeploymentConfig
    {
        public string Namespace { get; set; }
        public string Image { get; set; }
        public string ImagePullPolicy { get; set; }
        public string ImagePullSecretsName { get; set; }
    }
}
