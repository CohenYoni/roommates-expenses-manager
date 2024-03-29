﻿using RoommatesExpensesManager.Dal;
using RoommatesExpensesManager.Models;
using RoommatesExpensesManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using RoommatesExpensesManager.ModelBinders;

namespace RoommatesExpensesManager.Controllers
{
    public class ManagerController : Controller
    {
        private bool Authorize()
        {
            //check if the user logged in as manager
            if (Session["CurrentUser"] == null ||
                (Session["CurrentUser"] != null && !((User)Session["CurrentUser"]).IsManager))
                return false;
            else
                return true;
        }

        public ActionResult ShowManagerPage()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return View();
        }

        public ActionResult AddGroup()
        {
            //add new roommate groupaction

            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            GroupDal grpDal = new GroupDal();
            VMGroups groupsVM = new VMGroups
            {
                Group = new Group(),
                Groups = grpDal.Groups.ToList<Group>()
            };
            return View(groupsVM);
        }

        public ActionResult AddGroupSubmit([ModelBinder(typeof(GroupBinder))] Group grp)
        {
            //add the new group to DB

            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            VMGroups groupsVM = new VMGroups();
            GroupDal grpDal = new GroupDal();
            ModelState.Clear();
            TryValidateModel(grp);
            if (ModelState.IsValid)
            {
                grp.managerUserName = ((User)(Session["CurrentUser"])).UserName;
                grpDal.Groups.Add(grp);
                try
                {
                    grpDal.SaveChanges();
                    //reset a new group object to view
                    groupsVM.Group = new Group();
                    GroupRoommate grpRmt = new GroupRoommate()
                    {
                        groupID = grp.gid,
                        userName = grp.managerUserName
                    };
                    GroupRoommateDal grpRmtDal = new GroupRoommateDal();
                    grpRmtDal.GroupsRoommates.Add(grpRmt);
                    grpRmtDal.SaveChanges();
                    ViewBag.addGroupSuccess = "הקבוצה התווספה בהצלחה!";
                }
                catch (DbUpdateException)
                {
                    ViewBag.addNewGroupError = "התרחשה שגיאה בהוספת הקבוצה";
                    groupsVM.Group = grp;
                }
            }
            else
                groupsVM.Group = grp;
            groupsVM.Groups = grpDal.Groups.ToList<Group>();
            return View("AddGroup", groupsVM);
        }

        public ActionResult AddCategory()
        {
            //add new category action. (asynchronous page)
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            Category ctgy = new Category();
            return View(ctgy);
        }

        public ActionResult GetCategoriesByJson()
        {
            //get json of all the categories from DB
            if (Session["CurrentUser"] == null)
                return RedirectToAction("RedirectByUser", "Home");
            CategoryDal ctgyDal = new CategoryDal();
            List<Category> categories = ctgyDal.Categories.ToList<Category>();
            Thread.Sleep(1000);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveNewCategory()
        {
            //save the new category
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            CategoryDal ctgyDal = new CategoryDal();
            Category newCategory = new Category
            {
                Type = Request.Form["Type"].ToString()
            };
            ModelState.Clear();
            TryValidateModel(newCategory);
            if (ModelState.IsValid)
            {
                try
                {
                ctgyDal.Categories.Add(newCategory);
                ctgyDal.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //TODO: show error message in client side
                    ViewBag.addNewCategoryError = "התרחשה שגיאה בהוספת הקטגוריה";
                }
            }
            else
                ViewBag.addNewCategoryError = "הזן קטגוריה תקינה!";
            List<Category> categories = ctgyDal.Categories.ToList<Category>();
            Thread.Sleep(1000);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUserToGroup()
        {
            //add registered users to roommate groups of the current manager
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User u = (User)(Session["CurrentUser"]);
            GroupDal groupd = new GroupDal();
            //get only the groups of the manager
            List<Group> MyGroups = (from g in groupd.Groups
                                    where g.managerUserName == u.UserName
                                    select g).ToList<Group>();
            VMGroups grp = new VMGroups() { Groups = MyGroups };
            return View(grp);
        }

        public ActionResult ChooseUser(Group grp)
        {
            //add users to choosen group
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User u = (User)(Session["CurrentUser"]);
            //get the choosen group details
            GroupRoommateDal grpRmtDal = new GroupRoommateDal();
            List<string> userNamesAlreadyInGrp = (from usr in grpRmtDal.GroupsRoommates
                                                  where usr.groupID == grp.gid
                                                  select usr.userName).ToList<string>();
            //get all the registered users except the current manager and the the users that already belongs to the group
            UserDal usrDal = new UserDal();
            List<User> usrs = (from usr in usrDal.Users
                                where usr.UserName != u.UserName
                                && !userNamesAlreadyInGrp.Contains(usr.UserName)
                               select usr).ToList<User>();
            VMUsersGroup usrsGidVM = new VMUsersGroup()
            {
                group = grp,
                users = usrs
            };
            return View(usrsGidVM);
        }

        public ActionResult SubmitChoosenUser()
        {
            //add the choosen users to DB
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            VMUsersGroup vmUsrGrp = (VMUsersGroup)TempData["VMUsersGroup"];
            if (Request.Form["users[]"] == null)
            {
                ViewBag.erorAddUser = "לא נבחר משתמש";
                return View("ChooseUser", vmUsrGrp);
            }
            string usrs = Request.Form["users[]"].ToString();
            List<string> userNames = usrs.Split(',').ToList<string>();
            GroupRoommateDal grpRmtDal = new GroupRoommateDal();
            foreach (string usrName in userNames)
                {
                    GroupRoommate newRec = new GroupRoommate()
                    {
                        groupID = vmUsrGrp.group.gid,
                        userName = usrName
                    };
                    grpRmtDal.GroupsRoommates.Add(newRec);
                }
            grpRmtDal.SaveChanges();
            ViewBag.addUsersToGroupSuccess = "המשתמשים התווספו בהצלחה!";
            return View("ShowManagerPage");
        }
    }
}