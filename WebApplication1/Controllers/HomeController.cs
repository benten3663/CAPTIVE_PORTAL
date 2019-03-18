using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using System.Xml;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.Models.Fortinet;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {


        #region user registration
        private readonly HttpClient httpClient = new HttpClient();

        private const string urlEmailRegistration = "https://preprod-bpcustomer.cs85.force.com/BP/services/apexrest/IdPUser";

        private const string FortinetLoginAPI = "https://86.22.184.150:29173/api/v1/localusers/";

        


        public HomeController()
        {
            httpClient.BaseAddress = new Uri("https://preprod-bpcustomer.cs85.force.com");
            httpClient.BaseAddress = new Uri("https://86.22.184.150:29173/");

            httpClient.Timeout = TimeSpan.FromMinutes(1);
        }

        [HttpGet]
        public ActionResult UserRegister()
        {

            return View();
        }

        [HttpPost]
        public ActionResult UserRegister(EmailUserRegistrationModel e)
        {
            if (ModelState.IsValid)
            {
                var result = EmailUserRegistration(e.FirstName, e.LastName, e.Email);
                return RedirectToAction("VerifyEmail");
            }
            else
            {
                return View(e);
            }


        }
        public ActionResult VerifyEmail()
        {
            return View();
        }

        public CommonResponse EmailUserRegistration(string firstName, string lastName, string email)
        {
            var requestBody = new Models.GenericRequestModel<Models.EmailUserRegistrationModel>
            {
                RequestData = new EmailUserRegistrationModel()
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, urlEmailRegistration)
            {
                Content = CreateHttpContent(requestBody)

            };

            var result = httpClient.SendAsync(request).Result;

            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<CommonResponse>(content);
        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Newtonsoft.Json.Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
        #endregion user registration

        #region User Login
        [HttpGet]
        public ActionResult UserLogin()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UserLogin(LoginModel l)
        {
            if (ModelState.IsValid)
            {
                //FortinetLogin1();
               FortinetGet G = await GetRequest(l.Username);

                
                //var _result = FortinetLogin();

                var result = LoginWebService(l.Username, l.Password);
                if (result == true)
                {
                    if (G.Meta.total_Count == 0)
                    {
                        FortinetPut p = await PostRequest(l.Username);
                    }

                    
                    else if (G.Objects[0].user_Groups.Contains("/api/v1/usergroups/10/"))
                    {
                        await PatchRequest(l.Username);
                    }

                    return RedirectToAction("successPage");
                }
                else
                    ViewBag.Message = "User name or password is wrong";
                return View(l);
            }
            else
            {
                return View(l);
            }
        }

        public ActionResult successPage()
        {
            return View();
        }


        public static bool LoginWebService(string userName, string pswd)
        {
            var _url = "https://idptest-bpcustomer.cs109.force.com/BP/services/Soap/c/44.0";
            var _action = "https://idptest-bpcustomer.cs109.force.com/BP/services/Soap/c/44.0";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(userName, pswd);

            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);


            asyncResult.AsyncWaitHandle.WaitOne();

            string soapResult;
            try
            {
                //WebResponse response = webRequest.GetResponse();
                //Stream responseStream = response.GetResponseStream();

                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }

                }
                return true;
            }
            catch (WebException e)
            {
                return false;
            }


        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);

            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string _userName, string _pswd)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:enterprise.soap.sforce.com""><soapenv:Header><urn:LoginScopeHeader><urn:organizationId>00D0Q0000008adq</urn:organizationId></urn:LoginScopeHeader></soapenv:Header><soapenv:Body><urn:login><urn:username>" + _userName + "</urn:username><urn:password>" + _pswd + "</urn:password></urn:login></soapenv:Body></soapenv:Envelope>");
            return soapEnvelopeDocument;

        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }



        //fortinet Get Request
        public async Task<FortinetGet> GetRequest(string userName)
        {
            var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true };


            using (var httpLocalClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://86.22.184.150:29173/api/v1/localusers/?username="+ userName))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("api_fac7:wwpobGBytakRlHrPcaDdQqEkFNyNNaLuCPCF0Fly"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64Authorization}");

                    var response = await httpLocalClient.SendAsync(request);

                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<FortinetGet>(content);
                }
            }
        }




        #endregion user login

        //fortinet Post Request
        public async Task<FortinetPut> PostRequest(string userName)
        {
            var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true };


            using (var httpLocalClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://86.22.184.150:29173/api/v1/localusers/"))
                {
                    var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("api_fac7:wwpobGBytakRlHrPcaDdQqEkFNyNNaLuCPCF0Fly"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64Authorization}");

                    Dictionary<string, string> jsonValues = new Dictionary<string, string>();

                    jsonValues.Add("active", "true");
                    jsonValues.Add("user_groups", "[\"/api/v1/usergroups/10/\"]");
                    jsonValues.Add("username", userName);
                    jsonValues.Add("password", "password");

                    request.Content = new StringContent(JsonConvert.SerializeObject(jsonValues), Encoding.UTF8, "application/json");

                    var response = await httpLocalClient.SendAsync(request);

                    var content = await response.Content.ReadAsStringAsync();


                    return JsonConvert.DeserializeObject<FortinetPut>(content);
                }
            }
        }


        //Fortinet Patch request
        public async Task<object> PatchRequest(string userName)
        {
            var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true };

            using (var httpLocalClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://86.22.184.150:29173/api/v1/localusers/17/"))
                {
                    var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("api_fac7:wwpobGBytakRlHrPcaDdQqEkFNyNNaLuCPCF0Fly"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64Authorization}");

                    Dictionary<string, string> jsonValues = new Dictionary<string, string>();

                    jsonValues.Add("active", "true");
                    jsonValues.Add("user_groups", "[\"/api/v1/usergroups/9/\"]");
                    jsonValues.Add("username", userName);
                    jsonValues.Add("password", "password");

                    request.Content = new StringContent(JsonConvert.SerializeObject(jsonValues), Encoding.UTF8, "application/json");

                    var response = await httpLocalClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<FortinetPut>(content);
                }
            }
        }
    }
}
