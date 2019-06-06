using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.Transaction
{
    public class SumOfCategory
    {    
        public string Name { get; set; }
        public Decimal Total { get; set; }  
    }
}