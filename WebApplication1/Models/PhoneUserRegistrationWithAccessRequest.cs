using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class PhoneUserRegistrationWithAccessRequest
    {
        [JsonProperty("reqData")]
        public PhoneUserRegistrationWithAccessModel ReqData { get; set; }
    }
}