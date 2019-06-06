using Newtonsoft.Json;
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
               
        public ActionResult Index()
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateHouseHold()
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }

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

            var token = cookie.Values;


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

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpGet]
        public ActionResult ListOfHouseHold()
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
                var model = JsonConvert.DeserializeObject<List<ListOfHouseHoldViewModel>>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpGet]
        public ActionResult DetailOfHouseHold(int id)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Values;

            var url = $"http://localhost:55336/api/Household/GetHouseHoldDetail/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<DetailOfHouseHoldViewModel>(data);
                return View(model);
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";            
                return RedirectToAction("ListOfHouseHold","HouseHold");
            }
            else
            {
                ModelState.AddModelError("Error", "Sorry, Internal server Error occured");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult EditHouseHold(int id)
        {
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/HouseHold/GetByOwnerId/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
           
            var response = httpClient.GetAsync(url).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
                  
            var data = response.Content.ReadAsStringAsync().Result;

            var model = JsonConvert.DeserializeObject<EditHouseHoldeViewModel>(data);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditHouseHold(EditHouseHoldeViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var cookie = Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/HouseHold/Update/{formData.Id}";


            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, enCodeParameters).Result;
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

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }           
        }

        [HttpGet]
        public ActionResult UsersOfHouseHold(int id)
        {
            var cookie = Request.Cookies["BBCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/GetAllMember/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }

            var data = response.Content.ReadAsStringAsync().Result;
            var models = JsonConvert.DeserializeObject<List<UsersOfHouseHoldViewModel>>(data);
            return View(models);
        }

        [HttpGet]
        public ActionResult InviteUser(int id)
        {
            ViewBag.HouseHoldId = id;

            return View();
        }

        [HttpPost]
        public ActionResult InviteUser(InviteUserViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HouseHoldId = formData.Id;
                return View();
            }
            var cookie = Request.Cookies["BBCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/InviteUser/{formData.Id}";


            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();           
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));

            var enCodeParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, enCodeParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "Invitation email has sent successfully!";
                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                foreach (var ele in result.ModelState)
                {
                    ModelState.AddModelError("", ele.Value[0].ToString());
                }

                ViewBag.HouseHoldId = formData.Id;

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }            
        }

        [HttpGet]
        public ActionResult ListOfInvitation()
        {
            var cookie = Request.Cookies["BBCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/GetAllInvitaion";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }

            var data = response.Content.ReadAsStringAsync().Result;

            var models= JsonConvert.DeserializeObject<List<ListOfInvitation>>(data);

            return View(models);
        }

        [HttpGet]
        public ActionResult AcceptInvitation(int id)
        {
            var cookie = Request.Cookies["BBCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/AcceptInvitaion/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            } 

            return View("CustomErrorView");
        }

        [HttpPost]
        public ActionResult LeaveHousehold(int id)
        {
            var cookie = Request.Cookies["BBCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/Leave/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.DeleteAsync(url).Result;
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
                              
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }

        [HttpPost]
        public ActionResult DeleteHouseHold(int id)
        {
            var cookie = Request.Cookies["BBCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "Account");
            }
            var token = cookie.Value;

            var url = $"http://localhost:55336/api/Household/Delete/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.DeleteAsync(url).Result;
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

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Sorry, InternalServerError was occured during processing your request");
                return View(ModelState);
            }
        }       

    }
}