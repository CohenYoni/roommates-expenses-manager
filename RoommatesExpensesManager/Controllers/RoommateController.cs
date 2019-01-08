using RoommatesExpensesManager.Dal;
using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoommatesExpensesManager.Controllers
{
    public class RoommateController : Controller
    {
        // GET: Roomate
        public ActionResult ShowRoommatePage()
        {
            User u = (User)(Session["CurrentUser"]);
            GroupRoommateDal gdal = new GroupRoommateDal();
            List<int> gids = (from g in gdal.Groups
                           where g.userName == u.UserName
                           select g.groupID).ToList<int>();
            GroupDal groupd = new GroupDal();
            List<Group> MyGroups = (from g in groupd.Groups
                                    where gids.Contains(g.gid)
                                    select g).ToList<Group>();
            VMUserGroups usr = new VMUserGroups(u);
            usr.MyGroups = MyGroups;
            return View(usr);
        }
        //public ActionResult ShowExpense()
        //{

        //}

   

    }
}