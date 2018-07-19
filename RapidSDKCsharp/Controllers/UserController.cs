using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using RapidSDKCsharp.Models;
using System.Web.Script.Serialization;
using System.Web;
using System.Net.Http.Headers;
using RapidSDKCSharp;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace RapidSDKCsharp.Controllers
{
    [System.Web.Http.RoutePrefix("api/User")]
    public class UserController : ApiController
    {
       
        //HttpPost
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            HttpClient client = await HTTP_GET();

            
            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("firstname"), HttpUtility.HtmlEncode(model.firstname)));
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("lastname"), HttpUtility.HtmlEncode(model.lastname)));
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("email"), HttpUtility.HtmlEncode(model.email)));
            postData.Append(String.Format("{0}={1}", HttpUtility.HtmlEncode("password"), HttpUtility.HtmlEncode(model.password)));
            StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage message = await client.PostAsync("/v1/register", myStringContent);
            
            return ResponseMessage(message);
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Http.Route("Login")]
        public async Task<IHttpActionResult> Login(LoginModel model)
        {
            HttpClient client = await HTTP_GET();


            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("username"), HttpUtility.HtmlEncode(model.username)));
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("password"), HttpUtility.HtmlEncode(model.password)));
            StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await client.PostAsync("/v1/login", myStringContent);

            // I need help on how to set session cookie

            var contents = await response.Content.ReadAsStringAsync();
            var contentArray = contents.Split(':', ',', '"');
            var cookie1 = new CookieHeaderValue("session_id", contentArray[4].ToString());
            cookie1.Expires = DateTimeOffset.Now.AddDays(1);
            var cookie2 = new CookieHeaderValue("user_id", contentArray[10].ToString());
            cookie2.Expires = DateTimeOffset.Now.AddDays(1);
            response.Headers.AddCookies(new CookieHeaderValue[] { cookie1,cookie2 });

            var cookieNameValues = new Dictionary<string, string>();
            var url = "http://localhost:59966";
            cookieNameValues.Add("session_id", contentArray[4].ToString());
            cookieNameValues.Add("user_id", contentArray[10].ToString());
            var htmlResult = DownloadString(url, Encoding.UTF8, cookieNameValues);


            return ResponseMessage(response);
        }

       
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Create")]
        public async Task<IHttpActionResult> Create(DataModel model)
        {
            //Session is always null. dont know how to fix this one
            var session_id = HttpContext.Current.Session.SessionID;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(model);
            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("session_id"), HttpUtility.HtmlEncode(session_id)));
            postData.Append(String.Format("{0}={1}", HttpUtility.HtmlEncode("data"), HttpUtility.HtmlEncode(jsonData)));
            StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpClient client = await HTTP_GET();
            HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/create", myStringContent);

            

            return ResponseMessage(message);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Update")]
        public async Task<IHttpActionResult> Update(DataModel model)
        {

            var session_id = HttpContext.Current.Session.SessionID;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(model);
            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("session_id"), HttpUtility.HtmlEncode(session_id)));
            postData.Append(String.Format("{0}={1}", HttpUtility.HtmlEncode("data"), HttpUtility.HtmlEncode(jsonData)));
            StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpClient client = await HTTP_GET();
            HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/update", myStringContent);



            return ResponseMessage(message);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Read")]
        public async Task<IHttpActionResult> Read(DataModel model)
        {

            var session_id = HttpContext.Current.Session.SessionID;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(model);
            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("session_id"), HttpUtility.HtmlEncode(session_id)));
            postData.Append(String.Format("{0}={1}", HttpUtility.HtmlEncode("data"), HttpUtility.HtmlEncode(jsonData)));
            StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpClient client = await HTTP_GET();
            HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/read", myStringContent);



            return ResponseMessage(message);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Delete")]
        public async Task<IHttpActionResult> Delete(DataModel model)
        {

            var session_id = HttpContext.Current.Session.SessionID;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(model);
            StringBuilder postData = new StringBuilder();
            postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("session_id"), HttpUtility.HtmlEncode(session_id)));
            postData.Append(String.Format("{0}={1}", HttpUtility.HtmlEncode("data"), HttpUtility.HtmlEncode(jsonData)));
            StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpClient client = await HTTP_GET();
            HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/delete", myStringContent);



            return ResponseMessage(message);
        }



        public static async Task<HttpClient> HTTP_GET()
        {
            var TARGETURL = "https://api.rapidsdk.com";



            Console.WriteLine("GET: + " + TARGETURL);

            // ... Use HttpClient.            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.rapidsdk.com");

            var byteArray = Encoding.ASCII.GetBytes("08f3284b9895b097e67c543168ff301ebc84fe164bb61442ba1a823caa797459:9785ac6154b56fbe8d2203580e9366bc41b830f1f996bfa5f121ce6ff34f8175");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.GetAsync(TARGETURL);
            HttpContent content = response.Content;

            // ... Check Status Code                                
            Console.WriteLine("Response StatusCode: " + (int)response.StatusCode);

            // ... Read the string.
            string result = await content.ReadAsStringAsync();

            if ((int)response.StatusCode == 200)
            {
                return client;
            }

            return null;

        }
        public static string DownloadString(string url, Encoding encoding, IDictionary<string, string> cookieNameValues)
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(url);
                var webRequest = WebRequest.Create(uri);
                foreach (var nameValue in cookieNameValues)
                {
                    TryAddCookie(webRequest, new Cookie(nameValue.Key, nameValue.Value, "/", uri.Host));
                }
                var response = webRequest.GetResponse();
                var receiveStream = response.GetResponseStream();
                var readStream = new StreamReader(receiveStream, encoding);
                var htmlCode = readStream.ReadToEnd();
                return htmlCode;
            }
        }

        public static bool TryAddCookie(WebRequest webRequest, Cookie cookie)
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }

            httpRequest.CookieContainer.Add(cookie);
            return true;
        }
    }
}
