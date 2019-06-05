using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.Category
{
    public class EditCategoryViewModel
    {
        public int HouseHoldId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}