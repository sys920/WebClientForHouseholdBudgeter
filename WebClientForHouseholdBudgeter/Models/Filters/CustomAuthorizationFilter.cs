using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace WebClientForHouseholdBudgeter.Models.Filters
{
    public class CustomAuthorizationFilter : ActionFilterAttribute
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
           
        }
    }
}