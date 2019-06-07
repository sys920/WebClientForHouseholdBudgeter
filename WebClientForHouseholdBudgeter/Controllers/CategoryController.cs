using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.ViewModels.HouserHold;
using WebClientForHouseholdBudgeter.Models.ViewModels.Category;
using WebClientForHouseholdBudgeter.Models.Filters;

namespace WebClientForHouseholdBudgeter.Controllers
{
    [CustomAuthorizationFilter]
    public class CategoryController : Controller
    {
        [HttpGet]
        public ActionResult ListOfHouseHoldForCategory()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/GetAll";
            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<ListOFHouseHoldForCategoryViewModel>>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOFHouseHoldForCategory", "Category");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult ListOfCategory(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/Category/GetAllCategory/{id}";

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<ListOfCategoryViewModel>>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOFHouseHoldForCategory", "Category");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult CreateCategory(int id)
        {
            ViewBag.HouseHoldId = id;

            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(int id, CreateCategoryViewModel formData )
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HouseHoldId = id;

                return View(formData);
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("HouseHoldId", id.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/Category/Create/";
            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Category was created successfully";
                return RedirectToAction("ListOfHouseHoldForCategory", "Category");
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
        public ActionResult EditCategory(int? id, int? householdId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/Category/GetById/{id}";
            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<EditCategoryViewModel>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfCategory", "Category", new { id = householdId });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditCategory(int id, EditCategoryViewModel formData)
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
            var url = $"http://localhost:55336/api/Category/Update/{id}";
            var response = httpClient.PutAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Category was edited successfully";
                return RedirectToAction("ListOfCategory", "Category", new { id = formData.HouseHoldId });
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
        public ActionResult DeleteCategory(int id, int householdId)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");
            var url = $"http://localhost:55336/api/Category/Delete/{id}";
            var response = httpClient.DeleteAsync(url).Result;

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Category was deleted successfully";
                return RedirectToAction("ListOfCategory", "Category", new { id = householdId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";

                return RedirectToAction("ListOfCategory", "Category", new { id = householdId });
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}