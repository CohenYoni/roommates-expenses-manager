using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    //[Table("group")]
    public class Group
    {
        [Key]
        public int gid { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public int aptNum { get; set; }
        public string managerUserName { get; set; }
    }
}