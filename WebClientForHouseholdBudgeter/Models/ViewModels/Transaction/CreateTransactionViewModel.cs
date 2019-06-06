using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebClientForHouseholdBudgeter.Models.ViewModels.Category;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.Transaction
{
    public class CreateTransactionViewModel
    {
        public int BankAccountId { get; set; }
        public int HouseHoldId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<CategoryNameList> CategoryNameList { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}