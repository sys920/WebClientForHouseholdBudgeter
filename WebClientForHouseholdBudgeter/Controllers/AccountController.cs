using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
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

                var result = JsonConvert.DeserializeObject<LoginToken>(data);

                var cookie = new HttpCookie("BBCookie",result.AccessToken);
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }



    }
}