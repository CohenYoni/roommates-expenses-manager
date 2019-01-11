using RoommatesExpensesManager.Dal;
using RoommatesExpensesManager.Models;
using RoommatesExpensesManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoommatesExpensesManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult RedirectByUser()
        {
            if (Session["CurrentUser"] != null)
            {
                User currentUsr = (User)(Session["CurrentUser"]);
                if (currentUsr.IsManager)
                    return RedirectToAction("ShowManagerPage", "Manager");
                else
                    return RedirectToAction("ShowRoommatePage", "Roommate");
            }
            else
            {
                TempData["notAuthorized"] = "אין הרשאה!";
                return RedirectToAction("HomePage");
            }
        }

        public ActionResult HomePage()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            UserLogin usr = new UserLogin();
            return View(usr);
        }

        [HttpPost]
        public ActionResult Login(UserLogin usr)
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
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
                objUser.Password = "";
                Session["CurrentUser"] = objUser;
                return RedirectToAction("RedirectByUser");
            }
            else
            {
                usr.Password = "";
                return View("HomePage", usr);
            }
        }

        public ActionResult Register()
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            VMUserRegister newUsr = new VMUserRegister();
            newUsr.NewUser = new User();
            return View(newUsr);
        }

        [HttpPost]
        public ActionResult RegisterSubmit(VMUserRegister usr)
        {
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            usr.NewUser.Password = usr.Password;
            ModelState.Clear();
            TryValidateModel(usr);
            if (ModelState.IsValid)
            {
                UserDal usrDal = new UserDal();

                User objUser = (from user in usrDal.Users
                                where user.UserName == usr.NewUser.UserName
                                select user).FirstOrDefault<User>();
                if (objUser != null)
                {
                    ViewBag.errorUserRegister = "שם המשתמש שבחרת קיים";
                    return View("Register", usr);
                }
                usrDal.Users.Add(usr.NewUser);
                usrDal.SaveChanges();
                ViewBag.registerSuccessMsg = "ההרשמה בוצעה בהצלחה!";
                return View("HomePage", usr.NewUser);
            }
            else
            {
                usr.Password = "";
                return View("Register", usr);
            }
        }

        public ActionResult Logout()
        {
            if (Session["CurrentUser"] != null)
                Session.Remove("CurrentUser");
            return RedirectToAction("HomePage");
        }
    }
}