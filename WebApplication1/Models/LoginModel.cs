using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LoginModel
    {

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        [JsonProperty("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]

        [JsonProperty("password")]
        public string Password { get; set; }

    }
}