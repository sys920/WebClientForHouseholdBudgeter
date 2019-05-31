﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.ViewModels.HouserHold;

namespace WebClientForHouseholdBudgeter.Controllers
{
    public class HouseHoldController : Controller
    {
        // GET: HouseHold
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateHouseHold()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateHouseHold(CreateHouseHoldViewModel formData)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/Create";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));           

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ViewHouseHold", "HouseHold");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                foreach (var ele in result.ModelState)
                {
                    ModelState.AddModelError("", ele.Value[0].ToString());
                }

                return View();
            }

            return RedirectToAction("InternalServerError", "Account");
        }


        [HttpGet]
        public ActionResult ViewHouseHold()
        {
            var cookie = Request.Cookies["BBCookie"];

            if(cookie == null)
            {
                return RedirectToAction("login","Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/GetAll";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var model = JsonConvert.DeserializeObject<List<ViewHouseHoldViewModel>>(data);

            return View(model);
        }

    }
}