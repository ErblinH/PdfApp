using Newtonsoft.Json;

namespace PdfApp.API.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(T data)
        {
            Result = data;
        }
        public ApiResponse(T data, Pagination pagination)
        {
            Result = data;
            ResultInfo = pagination;
        }
        public bool Success { get; set; } = true;
        public IList<string> Errors { get; set; } = new List<string>();
        public IList<string> Messages { get; set; } = new List<string>();
        public T Result { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Pagination ResultInfo { get; set; } = null;
    }
}
