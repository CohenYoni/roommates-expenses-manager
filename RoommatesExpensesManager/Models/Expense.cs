using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    public class Expense
    {
        [Key]
        public int eid { get; set; }
        
        public string UserName { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        public double? Amount { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        [StringLength(20, ErrorMessage = "לכל היותר 20 תווים")]
        public string Store { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        public string Category { get; set; }
        public string referanceNum { get; set; }
        public string Comment { get; set; }
        
        public DateTime? expDate { get; set; }
        
    
        public DateTime EntryDate { get; set; }
       
        public int groupID { get; set; }

    }
}