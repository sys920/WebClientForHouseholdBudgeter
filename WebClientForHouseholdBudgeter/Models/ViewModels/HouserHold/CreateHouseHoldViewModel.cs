﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.HouserHold
{
    public class CreateHouseHoldViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}