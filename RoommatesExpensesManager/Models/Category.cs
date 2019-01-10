﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    public class Category
    {
        [Key]
        [Required (ErrorMessage = "שדה חובה")]
        public string Type { get; set; }
    }
}