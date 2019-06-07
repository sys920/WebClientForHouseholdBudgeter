using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.Filters;
using WebClientForHouseholdBudgeter.Models.ViewModels.BankAccount;

namespace WebClientForHouseholdBudgeter.Controllers
{
    [CustomAuthorizationFilter]
    public class BankAccountController : Controller
    {
        [HttpGet]
        public ActionResult ListOfHouseHoldForBankAccount()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/Household/GetAll";
            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<ListOFHouseHoldForBankAccountViewModel>>(data);
                return View(model);
            }
            else         
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult CreateBankAccount(int id)
        {            
            ViewBag.HouseHoldId = id;

            return View();
        }

        [HttpPost]
        public ActionResult CreateBankAccount(int id, CreateBankAccountViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HouseHoldId = id;

                return View();
            }
           
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("HouseHoldId", id.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/BankAccount/Create/";
            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "BankAccount was created successfully";
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

                return View(formData);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult ListOfBankAccount(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/BankAccount/GetAll/{id}";
            var response = httpClient.GetAsync(url).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<ListOfBankAccountViewModel>>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOFHouseHoldForBankAccount","BankAccount");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult EditBankAccount(int id, int houseHoldId)
        {   
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/BankAccount/GetById/{id}";
            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var model = JsonConvert.DeserializeObject<EditBankAccountViewModel>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfBankAccount", "BankAccount", new {id =houseHoldId });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditBankAccount(int id, EditBankAccountViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/BankAccount/Update/{id}";
            var response = httpClient.PutAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "BankAccount was edited successfully";
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
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult DetailOfBankAccount(int id, int houseHoldId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/BankAccount/GetById/{id}";
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<DetailOfBankAccountViewModel>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = houseHoldId });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public ActionResult DeleteBankAccount(int id, int householdId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/BankAccount/Delete/{id}";
            var response = httpClient.DeleteAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "BankAccount was deleted successfully";
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = householdId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = householdId });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult CalcurateBalance(int id, int houseHoldId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/BankAccount/CalcurateBalance/{id}";
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "This BankAccount was recalcurated successfully";
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = houseHoldId });
            }
             else
            {
                return RedirectToAction("Error", "Home");
            }
        }        
    }
}