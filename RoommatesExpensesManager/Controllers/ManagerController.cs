using RoommatesExpensesManager.Dal;
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
                    groupsVM.Group = new Group();
                    GroupRoommate grpRmt = new GroupRoommate()
                    {
                        groupID = grp.gid,
                        userName = grp.managerUserName
                    };
                    GroupRoommateDal grpRmtDal = new GroupRoommateDal();
                    grpRmtDal.GroupsRoommates.Add(grpRmt);
                    grpRmtDal.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //show error message in client side
                    ViewBag.addNewCategoryError = "התרחשה שגיאה בהוספת הקבוצה";
                    groupsVM.Group = grp;
                }
            }
            else
            {
                //groupsVM.Group = newGroup;
                groupsVM.Group = grp;
            }
            groupsVM.Groups = grpDal.Groups.ToList<Group>();
            return View("AddGroup", groupsVM);
        }

        public ActionResult AddCategory()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            Category ctgy = new Category();
            return View(ctgy);
        }

        public ActionResult GetCategoriesByJson()
        {
            if (Session["CurrentUser"] == null)
                return RedirectToAction("RedirectByUser", "Home");
            CategoryDal ctgyDal = new CategoryDal();
            List<Category> categories = ctgyDal.Categories.ToList<Category>();
            Thread.Sleep(1000);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveNewCategory()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            CategoryDal ctgyDal = new CategoryDal();
            Category newCategory = new Category
            {
                Type = Request.Form["Type"].ToString()
            };
            if (ModelState.IsValid)
            {
                try
                {
                ctgyDal.Categories.Add(newCategory);
                ctgyDal.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //show error message in client side
                    ViewBag.addNewCategoryError = "התרחשה שגיאה בהוספת הקטגוריה";
                }
            }
            List<Category> categories = ctgyDal.Categories.ToList<Category>();
            Thread.Sleep(1000);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUserToGroup()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User u = (User)(Session["CurrentUser"]);
            GroupDal groupd = new GroupDal();
            List<Group> MyGroups = (from g in groupd.Groups
                                    where g.managerUserName == u.UserName
                                    select g).ToList<Group>();
            VMGroups grp = new VMGroups() { Groups = MyGroups };
            return View(grp);
        }

        public ActionResult ChooseUser(Group grp)
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            User u = (User)(Session["CurrentUser"]);
            GroupRoommateDal grpRmtDal = new GroupRoommateDal();
            List<string> userNamesAlreadyInGrp = (from usr in grpRmtDal.GroupsRoommates
                                                  where usr.groupID == grp.gid
                                                  select usr.userName).ToList<string>();
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

        public ActionResult SubmitChoosenUser(VMUsersGroup vmUsrGrp)
        {
            string usrs;
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            Int32 grpID = (Int32)TempData["groupID"];
            if (Request.Form["users[]"] != null)
            {
                usrs = Request.Form["users[]"].ToString();
            }
            else
            {
                ViewBag.erorAddUser = "לא התווסף משתמש";
                GroupDal grpDal = new GroupDal();
                Group grp = (from g in grpDal.Groups
                             where g.gid == grpID
                             select g).First<Group>();
                return RedirectToAction("ChooseUser",grp);
            }
            List<string> userNames = usrs.Split(',').ToList<string>();
            GroupRoommateDal grpRmtDal = new GroupRoommateDal();
            foreach (string usrName in userNames)
                {
                    GroupRoommate newRec = new GroupRoommate()
                    {
                        groupID = grpID,
                        userName = usrName
                    };
                    grpRmtDal.GroupsRoommates.Add(newRec);
                }
            grpRmtDal.SaveChanges();
            //TODO: add success message
            return View("ShowManagerPage");
        }
    }
}