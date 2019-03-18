using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class PhoneUserRegistrationRequest
    {
        [JsonProperty("reqData")]
        public PhoneUserRegistrationModel ReqData { get; set; }
    }
}