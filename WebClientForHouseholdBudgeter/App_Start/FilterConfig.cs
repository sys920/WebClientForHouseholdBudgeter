using System.Web;
using System.Web.Mvc;
using WebClientForHouseholdBudgeter.Models.Filters;

namespace WebClientForHouseholdBudgeter
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthFilter());
        }
    }
}
