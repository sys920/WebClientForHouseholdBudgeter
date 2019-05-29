using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models.ViewModels.Account;

namespace WebClientForHouseholdBudgeter.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserViewModel formData)
        {
            

            return View();
        }
    }
}