using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.ViewModels.Account;

namespace WebClientForHouseholdBudgeter.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserViewModel formData)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var url = $"http://localhost:55336/api/Account/Register";
            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));
            parameters.Add(new KeyValuePair<string, string>("Password", formData.Password));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.OK)              
            {
                var data = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction("Login", "Account");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

               foreach(var ele in result.ModelState)
                {
                    ModelState.AddModelError("", ele.Value[0].ToString());  
                } 

                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = $"http://localhost:55336/Token";
            var grantType = "password";

            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", formData.Email));
            parameters.Add(new KeyValuePair<string, string>("Password", formData.Password));
            parameters.Add(new KeyValuePair<string, string>("grant_type", grantType));


            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<LoginTokenViewModel>(data);

                var cookie = new HttpCookie("BBCookie",result.AccessToken);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);
            }

            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = $"http://localhost:55336/api/Account/ChangePassword";           

            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("OldPassword", formData.OldPassword));
            parameters.Add(new KeyValuePair<string, string>("NewPassword", formData.NewPassword));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));    

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login","Account");
            }
            var token = cookie.Value;    
         
            httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");   

            var response = httpClient.PostAsync(url, enCodeParameters).Result;
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            { 
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = $"http://localhost:55336/api/Account/ForgotPassword";

            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));          

            var enCodeParameters = new FormUrlEncodedContent(parameters);                  

            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("SetPassword", "Account");
            }

            return RedirectToAction("ForgetPassword", "Account");
        }

        [HttpGet]
        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetPassword(SetPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = $"http://localhost:55336/api/Account/SetPassword";

            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));
            parameters.Add(new KeyValuePair<string, string>("Code", formData.Code));
            parameters.Add(new KeyValuePair<string, string>("NewPassword", formData.NewPassword));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("ForgetPassword", "Account");
        }
    }
}