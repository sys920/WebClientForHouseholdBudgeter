using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebClientForHouseholdBudgeter.Models.ViewModels.BankAccount;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.HouserHold
{
    public class DetailOfHouseHoldViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsOwner { get; set; }

        public List<BankAccountListForHousehold> bankAccountListForHouseholds { get; set; }

    }
}