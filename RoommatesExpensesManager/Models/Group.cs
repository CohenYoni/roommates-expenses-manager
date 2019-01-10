using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    public class Group
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int gid { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required (ErrorMessage = "שדה חובה")]
        public string city { get; set; }
        [Key]
        [Column(Order = 3)]
        [Required (ErrorMessage = "שדה חובה")]
        public string street { get; set; }
        [Key]
        [Column(Order = 4)]
        [Required(ErrorMessage = "שדה חובה")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "מספר הדירה מורכב ממספרים בלבד")]
        public int? aptNum { get; set; }
        public string managerUserName { get; set; }
    }
}