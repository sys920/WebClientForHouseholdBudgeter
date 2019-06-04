﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.HouserHold
{
    public class ListOfHouseHoldViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }      
        public bool IsOwner { get; set; }

        public int NumberOfMember { get; set; } 
    }
}