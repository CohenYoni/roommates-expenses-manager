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
        [Required (ErrorMessage = "שדה חובה")]
        public string city { get; set; }
        [Required (ErrorMessage = "שדה חובה")]
        public string street { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        public int aptNum { get; set; }
        public string managerUserName { get; set; }
    }
}