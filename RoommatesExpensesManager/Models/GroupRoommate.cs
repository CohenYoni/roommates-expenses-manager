using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Models
{
    [Table("groupRoommates")]
    public class GroupRoommate
    {
        [Key, Column(Order = 0)]
        public int groupID { get; set; }
        [Key, Column(Order = 1)]
        public string userName { get; set; }
    }
}