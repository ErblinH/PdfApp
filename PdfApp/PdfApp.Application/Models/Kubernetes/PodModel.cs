using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Models.Kubernetes
{
    public class PodModel
    {
        public string Name { get; set; }
        public int Replicas { get; set; }
    }
}
