using RoommatesExpensesManager.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.ViewModel
{
    public class VMExpense
    {
        public int gid { get; set; }
        public  Expense expense { get; set; }
        public VMExpense(int g)
        {
            gid = g;
            expense = new Expense();
        }
    }
}