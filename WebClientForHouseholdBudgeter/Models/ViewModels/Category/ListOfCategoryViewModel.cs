﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClientForHouseholdBudgeter.Models.ViewModels.Category
{
    public class ListOfCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }      
       
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsOwner { get; set; }
        public int HouseHoldId { get; set; }
    }
}