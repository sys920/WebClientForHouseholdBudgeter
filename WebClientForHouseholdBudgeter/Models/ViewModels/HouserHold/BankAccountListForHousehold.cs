using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClientForHouseholdBudgeter.Models.ViewModels.Transaction;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.BankAccount
{
    public class BankAccountListForHousehold
    {

        public int BankAccountId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
      
        public decimal Balance { get; set; }
        
        public List <SumOfCategory> SumOfCategories { get; set; }
    }
}