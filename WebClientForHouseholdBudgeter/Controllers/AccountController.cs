using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.Filters;
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
                return View(formData);
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
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }              
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
                return View(formData);
            }
           
            var grantType = "password";

            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", formData.Email));
            parameters.Add(new KeyValuePair<string, string>("Password", formData.Password));
            parameters.Add(new KeyValuePair<string, string>("grant_type", grantType));

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/Token";
            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<LoginTokenViewModel>(data);

                var cookie = new HttpCookie("BBCookie", result.AccessToken);
                Response.Cookies.Add(cookie);

                var cookie2 = new HttpCookie("UserEmail", formData.Email);
                Response.Cookies.Add(cookie2);

                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                ModelState.AddModelError("", result.ErrorDescription);
                return View();

            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet] 
        public ActionResult LogOut()
        {
            var cookie = new HttpCookie("BBCookie");
            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);

            var cookie2 = new HttpCookie("UserEmail");
            cookie2.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("login", "Account");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {            
            return View();
        }

        [CustomAuthorizationFilter]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel formData)
        {   
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("OldPassword", formData.OldPassword));
            parameters.Add(new KeyValuePair<string, string>("NewPassword", formData.NewPassword));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));    

            var enCodeParameters = new FormUrlEncodedContent(parameters); 
            var url = $"http://localhost:55336/api/Account/ChangePassword";
            var response = httpClient.PostAsync(url, enCodeParameters).Result;
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            { 
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                foreach (var ele in result.ModelState)
                {
                    ModelState.AddModelError("", ele.Value[0].ToString());
                }

                return View(formData);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
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
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                foreach (var ele in result.ModelState)
                {
                    ModelState.AddModelError("", ele.Value[0].ToString());
                }
                return View(formData);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
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
                return View(formData);
            }

            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));
            parameters.Add(new KeyValuePair<string, string>("Code", formData.Code));
            parameters.Add(new KeyValuePair<string, string>("NewPassword", formData.NewPassword));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var url = $"http://localhost:55336/api/Account/SetPassword";
            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Login", "Account");
            }
            else if(response.StatusCode ==System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);
                foreach(var ele in result.ModelState)
                {
                    ModelState.AddModelError("", ele.Value[0].ToString());
                }
                return View(formData);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }       
    }
}