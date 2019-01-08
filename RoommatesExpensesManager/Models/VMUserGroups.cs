using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    public class VMUserGroups
    {
        public User usr { get; set; }
        public List<Group> MyGroups { get; set; }

        public VMUserGroups(User u)
        {
            usr = u;
            MyGroups = new List<Group>();
        }
       
    }


}