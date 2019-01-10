using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.ViewModel
{
    public class VMUsersGroup
    {
        public Group group { get; set; }
        public List<User> users { get; set; }
    }
}