using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class EmailUserRegistrationModel
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}