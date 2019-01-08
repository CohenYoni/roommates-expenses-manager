using RoommatesExpensesManager.Dal;
using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoommatesExpensesManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult HomePage()
        {
            UserLogin usr = new UserLogin();
            return View(usr);
        }

        [HttpPost]
        public ActionResult Login(UserLogin usr)
        {
            if (ModelState.IsValid)
            {
                UserDal usrDal = new UserDal();

                User objUser = (from user in usrDal.Users
                                       where user.UserName == usr.UserName
                                       select user).FirstOrDefault<User>();
                if (objUser == null || objUser.Password != usr.Password)
                {
                    ViewBag.errorUserLogin = "המשתמש או הסיסמה שגויים";
                    return View("HomePage", usr);
                }
                Session["CurrentUser"] = objUser;
                if (objUser.IsManager)
                    return RedirectToAction("ShowManagerPage", "Manager");
                else
                    return RedirectToAction("ShowRoommatePage", "Roommate");
            }
            else
            {
                usr.Password = "";
                return View("HomePage", usr);
            }
        }

        public ActionResult Register()
        {
            User newUsr = new User();
            return View(newUsr);
        }

        [HttpPost]
        public ActionResult RegisterSubmit(User usr)
        {
            if (ModelState.IsValid)
                return View("Register", usr);
            else
            {
                usr.Password = "";
                return View("HomePage", usr);
            }
        }
    }
}