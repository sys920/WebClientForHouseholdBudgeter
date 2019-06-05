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

namespace WebClientForHouseholdBudgeter.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public ActionResult ListOfHouseHoldForCategory()
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

            var data = httpClient.GetStringAsync(url).Result;

            var model = JsonConvert.DeserializeObject<List<ListOFHouseHoldForCategoryViewModel>>(data);

            return View(model);
        }

        [HttpGet]
        public ActionResult ListOfCategory(int id)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/Category/GetAllCategory/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var model = JsonConvert.DeserializeObject<List<ListOfCategoryViewModel>>(data);

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateCategory(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("ListOfHouseHoldForCategory", "Category");
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
        public ActionResult CreateCategory(int id, CreateCategoryViewModel formData )
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

            var url = $"http://localhost:55336/api/Category/Create/";

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

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForCategory", "Category");
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Category/GetById/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOfCategory", "Category");
            }

            var data = response.Content.ReadAsStringAsync().Result;

            var model = JsonConvert.DeserializeObject<EditCategoryViewModel>(data);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditCategory(int? id, EditCategoryViewModel formData)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForCategory", "Category");
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

            var url = $"http://localhost:55336/api/Category/Update/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();           
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
      
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
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int? id, int householdId)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHoldForCategory", "Category");
            }

            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Category/Delete/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.DeleteAsync(url).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ListOfCategory", "Category", new { id = householdId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                TempData["Message"] = "Sorry, Your category or household doesn't exist";

                return RedirectToAction("ListOfCategory", "Category", new { id = householdId });
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }
    }
}