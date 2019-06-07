using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.Filters;
using WebClientForHouseholdBudgeter.Models.ViewModels.Category;
using WebClientForHouseholdBudgeter.Models.ViewModels.Transaction;

namespace WebClientForHouseholdBudgeter.Controllers
{
    [CustomAuthorizationFilter]
    public class TransactionController : Controller
    {
        [HttpGet]
        public ActionResult CreateTransaction(int id, int houseHoldId)
        {
            ViewBag.BankAccountId = id;
            ViewBag.HouseHoldId = houseHoldId;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

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
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreateTransaction (CreateTransactionViewModel formData)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

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

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", formData.BankAccountId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));
            parameters.Add(new KeyValuePair<string, string>("Date", formData.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", formData.Amount.ToString())); 

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/Transaction/Create/";
            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Transaction was createded successfully";
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
        public ActionResult ListOfTransaction(int id, int householdId)
        {
            ViewBag.HouseHoldId = householdId;

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Transaction/GetAll/{id}";
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<TransactionListViewModel>>(data); 

                return View(model);
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
        public ActionResult EditTransaction(int id, int houseHoldId)
        {                        
            ViewBag.HouseHoldId = houseHoldId;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

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
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfBankAccount", "BankAccount", new { id = houseHoldId });
            }
            else
            {                
                return RedirectToAction("Error", "Home");
            }            
        }

        [HttpPost]
        public ActionResult EditTransaction(int id, EditTransactionViewModel formData)
        {           
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url_Category = $"http://localhost:55336/api/Category/GetAllCategory/{formData.HouseHoldId}";
            var response_Category = httpClient.GetAsync(url_Category).Result;
            var data_Category = response_Category.Content.ReadAsStringAsync().Result;
            var categoryNameList = JsonConvert.DeserializeObject<List<CategoryNameList>>(data_Category);
            formData.CategoryNameList = categoryNameList;

            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));
            parameters.Add(new KeyValuePair<string, string>("Date", formData.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", formData.Amount.ToString()));

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/Transaction/Update/{id}";
            var response = httpClient.PutAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Transaction was edited successfully";
                return RedirectToAction("ListOfTransaction", "Transaction", new { id = formData.BankAccountId, houseHoldId=formData.HouseHoldId });
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

        [HttpPost]
        public ActionResult DeleteTransaction(int id, int bankAccountId, int houseHoldId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/Transaction/Delete/{id}";
            var response = httpClient.DeleteAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Transaction was deleted successfully";
                return RedirectToAction("ListOfTransaction", "Transaction", new { id = bankAccountId, houseHoldId = houseHoldId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfTransaction", "Transaction", new { id = bankAccountId, houseHoldId = houseHoldId });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult VoidTransaction(int id, int bankAccountId, int houseHoldId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Transaction/Void/{id}";
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Transaction was voided successfully";
                return RedirectToAction("ListOfTransaction", "Transaction", new { id = bankAccountId, houseHoldId = houseHoldId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfTransaction", "Transaction", new { id = bankAccountId, houseHoldId = houseHoldId });            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}