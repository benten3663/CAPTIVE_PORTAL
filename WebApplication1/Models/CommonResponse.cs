using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class CommonResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
    }
}