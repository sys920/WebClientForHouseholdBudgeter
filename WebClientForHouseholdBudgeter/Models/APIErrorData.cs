using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models
{
    public class APIErrorData
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> ModelState { get; set; }

        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}