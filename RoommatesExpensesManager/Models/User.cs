using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    public class User : UserLogin
    {
        [Required(ErrorMessage = "שדה חובה")]
        [StringLength(20, ErrorMessage = "לכל היותר 20 תווים")]
        public string FirstName  { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        [StringLength(20, ErrorMessage = "לכל היותר 20 תווים")]
        public string LastName   { get; set; }
        public bool isManager { get; set; }
    }
}