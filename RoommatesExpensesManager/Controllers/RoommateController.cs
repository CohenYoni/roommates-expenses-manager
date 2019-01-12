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

namespace RoommatesExpensesManager.Controllers
{
    public class RoommateController : Controller
    {
        private bool Authorize()
        {
            //check if the user logged in
            if (Session["CurrentUser"] == null)
                return false;
            else
                return true;
        }

        public ActionResult ShowRoommatePage()
        {
            //show the home page of roommates
            if(!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            //remove gid from the session (added at showExpenses action)
            Session.Remove("gid");
            //get the groups of the user from DB
            User u = (User)(Session["CurrentUser"]);
            GroupRoommateDal gdal = new GroupRoommateDal();
            List<int> gids = (from g in gdal.GroupsRoommates
                              where g.userName == u.UserName
                              select g.groupID).ToList<int>();
            GroupDal groupd = new GroupDal();
            List<Group> MyGroups = (from g in groupd.Groups
                                    where gids.Contains(g.gid)
                                    select g).ToList<Group>();
            VMGroups grp = new VMGroups() { Groups = MyGroups };
            return View(grp);
        }

        public ActionResult ShowExpenses(int gid)
        {
            //show the expenses of the choosen group (asynchronous page)
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            Expense exp = new Expense();
            Session["gid"] = gid;
            return View(exp);
        }

        public ActionResult GetExpensesByJson()
        {
            //get json of all the expenses of the group
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            int gid = (int)Session["gid"];
            ExpenseDal expDal = new ExpenseDal();
            List<Expense> expenseGroup = (from e in expDal.Expenses
                                          where e.groupID == gid
                                          select e).ToList<Expense>();
            Thread.Sleep(2000);
            return Json(expenseGroup, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveNewExpense()
        {
            //save new expense in DB and return json of all expenses
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            ExpenseDal expDal = new ExpenseDal();
            Expense newExpense = new Expense();
            try
            {
                newExpense.UserName = ((User)(Session["CurrentUser"])).UserName;
                newExpense.Amount = Convert.ToDouble(Request.Form["Amount"]);
                newExpense.Store = Request.Form["Store"];
                newExpense.Category = Request.Form["categoryCombo"];
                newExpense.referenceNum = Request.Form["referenceNum"];
                newExpense.Comment = Request.Form["Comment"];
                newExpense.expDate = Convert.ToDateTime(Request.Form["expDate"]);
                newExpense.EntryDate = DateTime.Now;
                newExpense.groupID = ((Int32)(Session["gid"]));
            }
            catch (Exception)
            {
                ViewBag.addNewCExpenseError = "שדות חובה!";
            }
            ModelState.Clear();
            TryValidateModel(newExpense);
            if (ModelState.IsValid)
            {
                //save the new expense
                try
                {
                    expDal.Expenses.Add(newExpense);
                    expDal.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    ViewBag.addNewCExpenseError = "התרחשה שגיאה בהוספת ההוצאה!";
                }
            }
            int gid = (int)Session["gid"];
            List<Expense> expenseGroup = (from e in expDal.Expenses
                                          where e.groupID == gid
                                          select e).ToList<Expense>();
            Thread.Sleep(2000);
            return Json(expenseGroup, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriesByJson()
        {
            //get categories json (from manager controller)
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return RedirectToAction("GetCategoriesByJson", "Manager");
        }
    }
}