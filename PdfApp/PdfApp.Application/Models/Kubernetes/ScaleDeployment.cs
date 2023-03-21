using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models.Kubernetes
{
    public class ScaleDeployment
    {
        public string Deployment { get; set; }
        public int Replicas { get; set; }
    }
}
