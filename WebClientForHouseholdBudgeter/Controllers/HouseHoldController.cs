using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models;
using WebClientForHouseholdBudgeter.Models.Filters;
using WebClientForHouseholdBudgeter.Models.ViewModels.HouserHold;

namespace WebClientForHouseholdBudgeter.Controllers
{
    [CustomAuthorizationFilter]
    public class HouseHoldController : Controller
    {
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
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description)); 
            
            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/Household/Create";                 
            var response = httpClient.PostAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "HouseHold was careated successfully";
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
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult ListOfHouseHold()
        {    
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/GetAll";
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
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult DetailOfHouseHold(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/GetHouseHoldDetail/{id}";
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
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult EditHouseHold(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/HouseHold/GetByOwnerId/{id}";
            var response = httpClient.GetAsync(url).Result;

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<EditHouseHoldeViewModel>(data);

                return View(model);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }   
        }

        [HttpPost]
        public ActionResult EditHouseHold(EditHouseHoldeViewModel formData)
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
            var url = $"http://localhost:55336/api/HouseHold/Update/{formData.Id}";
            var response = httpClient.PutAsync(url, enCodeParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "HouseHold was edited successfully";
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
        public ActionResult UsersOfHouseHold(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/GetAllMember/{id}";
            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var models = JsonConvert.DeserializeObject<List<UsersOfHouseHoldViewModel>>(data);
                return View(models);

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult InviteUser(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }

            ViewBag.HouseHoldId = id;

            return View();
        }

        [HttpPost]
        public ActionResult InviteUser(InviteUserViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HouseHoldId = formData.Id;

                return View(formData);
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var parameters = new List<KeyValuePair<string, string>>();           
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));

            var enCodeParameters = new FormUrlEncodedContent(parameters);
            var url = $"http://localhost:55336/api/Household/InviteUser/{formData.Id}";
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

                return View(formData);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }            
        }

        [HttpGet]
        public ActionResult ListOfInvitation()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/GetAllInvitaion";
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var models = JsonConvert.DeserializeObject<List<ListOfInvitation>>(data);

                return View(models);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }           
        }

        [HttpGet]
        public ActionResult AcceptInvitation(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/AcceptInvitaion/{id}";
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "You have joined the houseHold";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult LeaveHousehold(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/Leave/{id}";
            var response = httpClient.DeleteAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "You have leaved houseHold successfully";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult DeleteHouseHold(int id)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ViewBag.Token}");

            var url = $"http://localhost:55336/api/Household/Delete/{id}";
            var response = httpClient.DeleteAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Message"] = "HouseHold was deleted successfully";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["Message"] = "Sorry, Invalid request";
                return RedirectToAction("ListOfHouseHold", "HouseHold");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        } 
    }
}