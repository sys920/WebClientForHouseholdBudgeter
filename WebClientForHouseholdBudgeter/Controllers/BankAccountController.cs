using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.ViewModels.BankAccount;

namespace WebClientForHouseholdBudgeter.Controllers
{
    public class BankAccountController : Controller
    {
        [HttpGet]
        public ActionResult ListOfHouseHoldForBankAccount()
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/Household/GetAll";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<ListOFHouseHoldForBankAccountViewModel>>(data);
                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOFHouseHoldForBankAccount", "BankAccount");
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpGet]
        public ActionResult CreateBankAccount(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }

            ViewBag.HouseHoldId = id;

            return View();
        }

        [HttpPost]
        public ActionResult CreateBankAccount(int? id, CreateBankAccountViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HouseHoldId = id;

                return View();
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/BankAccount/Create/";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("HouseHoldId", id.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
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
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpGet]
        public ActionResult ListOfBankAccount(int id)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/BankAccount/GetAll/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<ListOfBankAccountViewModel>>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {                
                return RedirectToAction("ListOFHouseHoldForBankAccount","BankAccount");
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, Internal server Error occured");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult EditBankAccount(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/BankAccount/GetById/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOfBankAccount", "BankAccount");
            }

            var data = response.Content.ReadAsStringAsync().Result;

            var model = JsonConvert.DeserializeObject<EditBankAccountViewModel>(data);

            return View(model);
        }


        [HttpGet]
        public ActionResult DetailOfBankAccount(int id)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/BankAccount/GetById/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<DetailOfBankAccountViewModel>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOFHouseHoldForBankAccount", "BankAccount");
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpPost]
        public ActionResult EditBankAccount(int? id, EditBankAccountViewModel formData)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
            }

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/BankAccount/Update/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = formData.HouseHoldId });
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
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpPost]
        public ActionResult DeleteBankAccount(int? id, int householdId)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/BankAccount/Delete/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.DeleteAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = householdId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                TempData["Message"] = "Sorry, Your category or household doesn't exist";

                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = householdId });
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpGet]
        public ActionResult CalcurateBalance(int id, int houseHoldId)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/BankAccount/CalcurateBalance/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {   
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = houseHoldId });
            }
             else
            {
                ModelState.AddModelError("Error", "Sorry, Internal server Error occured");
                return RedirectToAction("Error", "Home");
            }
        }
        
    }
}