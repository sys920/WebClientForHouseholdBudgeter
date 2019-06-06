using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.ViewModels.Category;
using WebClientForHouseholdBudgeter.Models.ViewModels.Transaction;

namespace WebClientForHouseholdBudgeter.Controllers
{
    public class TransactionController : Controller
    {
        [HttpGet]
        public ActionResult CreateTransaction(int? id, int? houseHoldId)
        {
            if (id == null || houseHoldId == null)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
            }

            ViewBag.BankAccountId = id;
            ViewBag.HouseHoldId = houseHoldId;

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;         

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var url = $"http://localhost:55336/api/Category/GetAllCategory/{houseHoldId}";

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var categoryNameList = JsonConvert.DeserializeObject<List<CategoryNameList>>(data);

               var model = new CreateTransactionViewModel();
                model.CategoryNameList = categoryNameList;

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                return RedirectToAction("ListOFHouseHoldForBankAccount", "BankAccount");
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, Internal server Error occured");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreateTransaction (CreateTransactionViewModel formData)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var url_Category = $"http://localhost:55336/api/Category/GetAllCategory/{formData.HouseHoldId}";
            var response_Category = httpClient.GetAsync(url_Category).Result;
            var data_Category = response_Category.Content.ReadAsStringAsync().Result;
            var categoryNameList = JsonConvert.DeserializeObject<List<CategoryNameList>>(data_Category);

            if (!ModelState.IsValid)
            {
                ViewBag.BankAccountId = formData.BankAccountId;
                ViewBag.HouseHoldId = formData.HouseHoldId;                         
                formData.CategoryNameList = categoryNameList;

                return View(formData);              
            }

            var url = $"http://localhost:55336/api/Transaction/Create/"; 

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", formData.BankAccountId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));
            parameters.Add(new KeyValuePair<string, string>("Date", formData.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", formData.Amount.ToString()));           

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;
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
                ModelState.AddModelError("Error", "Sorry, Internal server Error occured");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult ListOfTransaction(int id, int householdId)
        {
            ViewBag.HouseHoldId = householdId;

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/Transaction/GetAll/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<TransactionListViewModel>>(data);
               

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                return RedirectToAction("ListOFBankAccount", "BankAccount");
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, Internal server Error occured");
                return RedirectToAction("Error", "Home");
            }        
        }

        [HttpGet]
        public ActionResult EditTransaction(int? id, int? houseHoldId)
        {
            if (id == null || houseHoldId == null)
            {
                return RedirectToAction("ListOfHouseHoldForBankAccount", "BankAccount");
            }

            ViewBag.BankAccountId = id;
            ViewBag.HouseHoldId = houseHoldId;

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");


            var url = $"http://localhost:55336/api/Transaction/GetById/{id}";
            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<EditTransactionViewModel>(data);

                var url_Category = $"http://localhost:55336/api/Category/GetAllCategory/{houseHoldId}";
                var response_Category = httpClient.GetAsync(url_Category).Result;
                var data_Category = response_Category.Content.ReadAsStringAsync().Result;
                var categoryNameList = JsonConvert.DeserializeObject<List<CategoryNameList>>(data_Category);
            
                model.CategoryNameList = categoryNameList;

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOFHouseHoldForBankAccount", "BankAccount");
            }
            else
            {                
                return RedirectToAction("Error", "Home");
            }
            
        }

    }
}