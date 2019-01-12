﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    public class UserLogin
    {
        [Key]
        [Required(ErrorMessage = "שדה חובה")]
        [RegularExpression("^[0-9a-zA-Z]+$", ErrorMessage = "שם המשתמש מורכב מאותיות אנגליות ומספרים בלבד")]
        [StringLength(20, ErrorMessage = "לכל היותר 20 תווים")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        public string Password { get; set; }
    }
}