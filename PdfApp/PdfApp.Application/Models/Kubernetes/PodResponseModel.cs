using PdfApp.Domain.Enums;

namespace PdfApp.Application.Models.Kubernetes
{
    public class PodResponseModel
    {
        public string UId { get; set; }
        public PodStatus Status { get; set; }
        public bool ContainerReadiness { get; set; }
    }
}
