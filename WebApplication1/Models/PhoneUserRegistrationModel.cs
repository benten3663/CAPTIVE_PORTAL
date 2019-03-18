using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class PhoneUserRegistrationModel
    {
        [JsonProperty("mobileRegistration")]
        public bool MobileRegistration { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("phoneCountryCode")]
        public string PhoneCountryCode { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }
    }
}