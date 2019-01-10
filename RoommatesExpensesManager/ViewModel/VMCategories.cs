using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.ViewModel
{
    public class VMCategories
    {
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
    }
}