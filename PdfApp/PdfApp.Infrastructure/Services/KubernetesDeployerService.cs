using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PdfApp.Application.Interfaces;
using PdfApp.Application.Models.Kubernetes;
using PdfApp.Domain.Configuration;
using PdfApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Services
{
    public class KubernetesDeployerService : IKubernetesDeployerService
    {

        private readonly Kubernetes _kubernetesClient;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<KubernetesDeployerService> _logger;
        private readonly DeploymentConfig _deploymentConfiguration;

        public KubernetesDeployerService(
            IOptions<AppSettings> appSettings,
            ILogger<KubernetesDeployerService> logger
        )
        {
            var applicationPath = "C:\\Users\\acer\\Desktop\\ushtrimet_docker\\live-encoder\\kubeconfig.yaml";
            var configObject = KubernetesClientConfiguration.LoadKubeConfig(applicationPath);
            var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(configObject);
            config.SkipTlsVerify = true;
            _kubernetesClient = new Kubernetes(config);
            _deploymentConfiguration = appSettings.Value.DeploymentConfig;
            _appSettings = appSettings;
            _logger = logger;
        }

        #region Launch deployment
        public async Task<PodResponseModel> LaunchDeploymentAsync(PodModel podModel)
        {
            try
            {
                var deplotmentModel = new V1Deployment()
                {
                    Metadata = new V1ObjectMeta()
                    {
                        Name = podModel.Name
                    },
                    Spec = new V1DeploymentSpec()
                    {
                        Replicas = podModel.Replicas,
                        Selector = new V1LabelSelector()
                        {
                            MatchLabels = new Dictionary<string, string>()
                            {
                                { "deploy" , "live" }
                            }
                        },
                        Template = new V1PodTemplateSpec()
                        {
                            Metadata = new V1ObjectMeta()
                            {
                                Labels = new Dictionary<string, string>()
                                {
                                    { "deploy" , "live" }
                                }
                            },
                            Spec = new V1PodSpec()
                            {
                                Containers = new List<V1Container>()
                                {
                                    new V1Container()
                                    {
                                        Name = podModel.Name,
                                        Image =_deploymentConfiguration.Image,
                                        ImagePullPolicy = _deploymentConfiguration.ImagePullPolicy,
                                        Resources = new V1ResourceRequirements
                                        {
                                            Limits = new Dictionary<string, ResourceQuantity>
                                            {
                                                { "cpu", new ResourceQuantity("10000m") },
                                                { "memory", new ResourceQuantity("1024Mi") },
                                            },
                                            Requests = new Dictionary<string, ResourceQuantity>
                                            {
                                                { "cpu", new ResourceQuantity("100m") },
                                                { "memory", new ResourceQuantity("64Ki") },
                                            }
                                        },
                                    }
                                },
                                ImagePullSecrets = new List<V1LocalObjectReference>
                                {
                                    new()
                                    {
                                        Name = _deploymentConfiguration.ImagePullSecretsName
                                    }
                                },
                                SecurityContext = new V1PodSecurityContext
                                {
                                    RunAsUser = 0
                                }
                            }
                        }
                    }

                };

                var response = await _kubernetesClient.CreateNamespacedDeploymentAsync(deplotmentModel, _deploymentConfiguration.Namespace);

                return new PodResponseModel
                {
                    UId = response.Uid()
                };

            }
            catch (Exception ex)
            {
                _logger.LogError("Error starting pod ex={ex}", ex);

                return null;
            }
        }
        #endregion

        #region Delete deployment
        public async Task<PodResponseModel> DeleteDeploymentAsync(string name)
        {
            try
            {
                var response =
                    await _kubernetesClient.DeleteNamespacedDeploymentAsync(name, _deploymentConfiguration.Namespace);

                return new PodResponseModel
                {
                    UId = response.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on deleting the pod podName={podName} - ex={ex}",
                    name, ex);

                return null;
            }
        }
        #endregion

        #region Scale Deployment
        public async Task<PodResponseModel> ScaleDeploymentAsync(ScaleDeployment scaleDeployment)
        {
            try
            {
                var jsonPatch = new JsonPatchDocument<V1Scale>();

                jsonPatch.Replace(e => e.Spec.Replicas, scaleDeployment.Replicas);

                var patch = new V1Patch(jsonPatch, V1Patch.PatchType.JsonPatch);

                var response =
                    await _kubernetesClient.PatchNamespacedDeploymentScaleAsync(patch, scaleDeployment.Deployment, _deploymentConfiguration.Namespace);

                return new PodResponseModel
                {
                    UId = response.Uid()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error scaling deployment ex={ex}", ex);
                return null;
            }
        }
        #endregion

        #region Pod Watcher
        public async Task<PodResponseModel> PodWatcherAsync(string podName)
        {
            try
            {
                var selectedPod = await _kubernetesClient.ReadNamespacedPodAsync(podName, _deploymentConfiguration.Namespace);

                if (selectedPod == null)
                    return new PodResponseModel() { Status = PodStatus.NotFound };

                var containerStatus = selectedPod.Status.ContainerStatuses.FirstOrDefault();

                Enum.TryParse(selectedPod.Status.Phase, true, out PodStatus podStatus);

                return new PodResponseModel()
                {
                    Status = podStatus,
                    ContainerReadiness = containerStatus is null ? false : containerStatus.Ready
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on observing pod status pod={podName} - ex={ex}", podName, ex);

                return new PodResponseModel() { Status = PodStatus.Error };
            }
        }
        #endregion
    }
}
