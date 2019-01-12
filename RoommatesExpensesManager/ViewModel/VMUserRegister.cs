using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.ViewModel
{
    public class VMUserRegister
    {
        //TODO: regex for password, at least one capial ,one number and one small letter
        [Required(ErrorMessage = "שדה חובה")]
        [RegularExpression("^[?!#%&*a-zA-Z0-9]{8,}$", ErrorMessage = "אותיות באנגלית, מספרים ותווים מיוחדים בלבד, לפחות 8 תווים")]
        public string Password { get; set; }
        //TODO: regex for password, at least one capial ,one number and one small letter
        [Required(ErrorMessage = "שדה חובה")]
        [RegularExpression("^[?!#%&*a-zA-Z0-9]{8,}$", ErrorMessage = "אותיות באנגלית, מספרים ותווים מיוחדים בלבד, לפחות 8 תווים")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "הזן את אותה סיסמה")]
        public string ConfirmPassword { get; set; }
        public User NewUser { get; set; }
    }
}