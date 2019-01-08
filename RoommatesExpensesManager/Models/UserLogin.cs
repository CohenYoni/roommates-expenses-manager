using System;
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
        //TODO: regex for password, at least one capial ,one number and one small letter
        [Required(ErrorMessage = "שדה חובה")]
        [RegularExpression("[a-zA-Z0-9]+$", ErrorMessage = "אותיות באנגלית, לפחות אחת גדולה ולפחות אחת קטנה, לפחות ספרה אחת")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "לפחות 8 תווים לכל היותר 20")]
        public string Password { get; set; }
    }
}