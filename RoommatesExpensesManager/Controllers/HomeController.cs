using RoommatesExpensesManager.Classes;
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
            //redirect to home page by the current user autorization
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
            //show the home page (with login)
            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            UserLogin usr = new UserLogin();
            return View(usr);
        }

        [HttpPost]
        public ActionResult Login(UserLogin usr)
        {
            //check if login successfully

            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            if (ModelState.IsValid)
            {
                //get the user from DB
                UserDal usrDal = new UserDal();
                Encryption enc = new Encryption();
                usr.UserName = usr.UserName.ToLower();
                User objUser = (from user in usrDal.Users
                                where user.UserName == usr.UserName
                                select user).FirstOrDefault<User>();

                //validate
                if (objUser == null || !enc.ValidatePassword(usr.Password, objUser.Password))
                {
                    ViewBag.errorUserLogin = "המשתמש או הסיסמה שגויים";
                    return View("HomePage", usr);
                }

                //go to home page
                objUser.Password = "";
                Session["CurrentUser"] = objUser;
                return RedirectToAction("RedirectByUser");
            }
            else
            {
                //go back to login page
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
            //check if register successfully

            if (Session["CurrentUser"] != null)
                return RedirectToAction("RedirectByUser");
            usr.NewUser.Password = usr.Password;
            usr.NewUser.UserName = usr.NewUser.UserName.ToLower();
            ModelState.Clear();
            TryValidateModel(usr);
            TryValidateModel(usr.NewUser);
            if (ModelState.IsValid)
            {
                //check if the username already exist
                UserDal usrDal = new UserDal();
                User objUser = (from user in usrDal.Users
                                where user.UserName == usr.NewUser.UserName
                                select user).FirstOrDefault<User>();
                if (objUser != null)
                {
                    ViewBag.errorUserRegister = "שם המשתמש שבחרת קיים";
                    return View("Register", usr);
                }

                //encrypt the password
                Encryption enc = new Encryption();
                string hashedPassword = enc.CreateHash(usr.NewUser.Password);
                usr.NewUser.Password = hashedPassword;
                usrDal.Users.Add(usr.NewUser);
                usrDal.SaveChanges();
                ViewBag.registerSuccessMsg = "ההרשמה בוצעה בהצלחה!";
                return View("HomePage", usr.NewUser);
            }
            else
            {
                //go back to registration page
                usr.Password = "";
                return View("Register", usr);
            }
        }

        public ActionResult Logout()
        {
            //logout. delete the session and go to home page
            if (Session["CurrentUser"] != null)
                Session.Remove("CurrentUser");
            return RedirectToAction("HomePage");
        }

        public ActionResult ShowDetails()
        {
            return View();
        }
    }
}