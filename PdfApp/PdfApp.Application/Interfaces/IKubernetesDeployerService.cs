using PdfApp.Application.Models.Kubernetes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Application.Interfaces
{
    public interface IKubernetesDeployerService
    {
        public Task<PodResponseModel> LaunchDeploymentAsync(PodModel podModel);
        public Task<PodResponseModel> DeleteDeploymentAsync(string name);
        public Task<PodResponseModel> ScaleDeploymentAsync(ScaleDeployment scaleDeployment);
        public Task<PodResponseModel> PodWatcherAsync(string podName);
    }
}
