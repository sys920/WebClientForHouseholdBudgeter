using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}