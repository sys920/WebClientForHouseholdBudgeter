using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace WebClientForHouseholdBudgeter.Models.Filters
{
    public class CustomAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var cookie = filterContext.HttpContext.Request.Cookies["BBCookie"];

            if (cookie == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new {action = "Login", Controller = "Account"}));
            }
            else
            {
                var token = cookie.Values;
                filterContext.Controller.ViewBag.Token = token;              
            }                     
        }
    }
}