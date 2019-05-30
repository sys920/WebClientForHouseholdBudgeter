using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.Account
{
    public class LoginTokenViewModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}