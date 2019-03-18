using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class EmailUserRegistrationRequest
    {
        [JsonProperty("reqData")]
        public EmailUserRegistrationModel RequestData { get; set; }
    }
}