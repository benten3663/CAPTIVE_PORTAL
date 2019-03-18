using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class GenericRequestModel<T>
    {
        [JsonProperty("reqData")]
        public T RequestData { get; set; }
    }
}