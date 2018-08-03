using System;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using RapidSDK.Models;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;

namespace RapidSDK.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]

        public async Task<ActionResult> Register(RegisterModel model)
        {
            try
            {
                HttpClient client = await HTTP_GET();


                StringBuilder postData = new StringBuilder();
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("firstname"), HttpUtility.HtmlEncode(model.firstname)));
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("lastname"), HttpUtility.HtmlEncode(model.lastname)));
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("email"), HttpUtility.HtmlEncode(model.email)));
                postData.Append(String.Format("{0}={1}", HttpUtility.HtmlEncode("password"), HttpUtility.HtmlEncode(model.password)));
                StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage message = await client.PostAsync("/v1/register", myStringContent);

                var contents = await message.Content.ReadAsStringAsync();
                return Content(contents);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                HttpClient client = await HTTP_GET();


                StringBuilder postData = new StringBuilder();
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("email"), HttpUtility.HtmlEncode(model.username)));
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("password"), HttpUtility.HtmlEncode(model.password)));
                StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.PostAsync("/v1/login", myStringContent);



                var contents = await response.Content.ReadAsStringAsync();
                var contentArray = contents.Split(':', ',', '"');
                var cookie1 = new HttpCookie("session_id", contentArray[4].ToString());
                cookie1.Expires = DateTime.Now.AddMinutes(5);
                var cookie2 = new HttpCookie("user_id", contentArray[10].ToString());
                cookie2.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie1);
                Response.Cookies.Add(cookie2);

                return Content(contents);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

        }



        [HttpPost]
        public async Task<ActionResult> Create(InputModel model)
        {

            try
            {
                var session_id = HttpContext.Request.Cookies["session_id"].Value;

                var data = model.data;
                var jsonData = JsonConvert.DeserializeObject(data);

                SendDataModel sendData = new SendDataModel();
                sendData.session_id = session_id;
                sendData.data = jsonData;

                HttpClient client = await HTTP_GET();
                HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/create", sendData);

                var contents = await message.Content.ReadAsStringAsync();
                return Content(contents);

            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Update(InputModel model)
        {
            ViewData["Message"] = null;
            try
            {
                var session_id = HttpContext.Request.Cookies["session_id"].Value;

                var data = model.data;
                var jsonData = JsonConvert.DeserializeObject(data);

                SendDataModel sendData = new SendDataModel();
                sendData.session_id = session_id;
                sendData.data = jsonData;

                HttpClient client = await HTTP_GET();
                HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/update", sendData);

                var contents = await message.Content.ReadAsStringAsync();

                return Content(contents);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Delete(InputModel model)
        {
            ViewData["Message"] = null;
            try
            {
                var session_id = HttpContext.Request.Cookies["session_id"].Value;
                var input = Regex.Replace(model.data, @"[^0-9a-zA-Z,]+", "");

                String[] list = input.Split(',');

                SendArray sendData = new SendArray();
                sendData.session_id = session_id;
                sendData.data = list;

                HttpClient client = await HTTP_GET();
                HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/delete", sendData);

                var contents = await message.Content.ReadAsStringAsync();

                return Content(contents);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Read(InputModel model)
        {

            try
            {
                var session_id = HttpContext.Request.Cookies["session_id"].Value;

                var input = Regex.Replace(model.data, @"[^0-9a-zA-Z,]+", "");

                String[] list = input.Split(',');

                SendArray sendData = new SendArray();
                sendData.session_id = session_id;
                sendData.data = list;

                HttpClient client = await HTTP_GET();
                HttpResponseMessage message = await client.PostAsJsonAsync("/v1/data/read", sendData);

                var contents = await message.Content.ReadAsStringAsync();
                return Content(contents);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {

            try
            {
                var session_id = HttpContext.Request.Cookies["session_id"].Value;
                var user_id = HttpContext.Request.Cookies["user_id"].Value;

                StringBuilder postData = new StringBuilder();
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("session_id"), HttpUtility.HtmlEncode(session_id)));
                postData.Append(String.Format("{0}={1}&", HttpUtility.HtmlEncode("user_id"), HttpUtility.HtmlEncode(user_id)));
                StringContent myStringContent = new StringContent(postData.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpClient client = await HTTP_GET();


                HttpResponseMessage message = await client.PostAsync("/v1/logout", myStringContent);

                var contents = await message.Content.ReadAsStringAsync();


                return Content(contents);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
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
    }
}
