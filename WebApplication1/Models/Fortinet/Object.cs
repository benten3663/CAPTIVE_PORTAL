using System.Collections.Generic;

namespace WebApplication1.Models.Fortinet
{
    public class Object
    {
        public bool Active { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Email { get; set; }
        public string ExpiresAt { get; set; }
        public string FirstName { get; set; }
        public bool FtkOnly { get; set; }
        public string FtmActMethod { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
        public string MailHost { get; set; }
        public string MailRoutingAddress { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool RecoveryByQuestion { get; set; }
        public string ResourceUri { get; set; }
        public string State { get; set; }
        public bool TokenAuth { get; set; }
        public bool TokenFas { get; set; }
        public string TokenSerial { get; set; }
        public string TokenType { get; set; }
        public IList<string> user_Groups { get; set; }
        public string Username { get; set; }
    }
}