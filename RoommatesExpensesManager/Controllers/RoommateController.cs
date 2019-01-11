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
            if (Session["CurrentUser"] == null)
                return false;
            else
                return true;
        }

        public ActionResult ShowRoommatePage()
        {
            if(!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            Session.Remove("gid");
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
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            Expense exp = new Expense();
            Session["gid"] = gid;
            return View(exp);
        }

        public ActionResult GetExpensesByJson()
        {
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
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            ExpenseDal expDal = new ExpenseDal();
            Expense newExpense = new Expense()
            {
                UserName = ((User)(Session["CurrentUser"])).UserName,
                Amount = Convert.ToDouble(Request.Form["Amount"]),
                Store = Request.Form["Store"],
                Category = Request.Form["categoryCombo"],
                referenceNum = Request.Form["referenceNum"],
                Comment = Request.Form["Comment"],
                expDate = Convert.ToDateTime(Request.Form["expDate"]),
                EntryDate = DateTime.Now,
                groupID = ((Int32)(Session["gid"]))
            };
            if (ModelState.IsValid)
            {
                try
                {
                    expDal.Expenses.Add(newExpense);
                    expDal.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    ViewBag.addNewCategoryError = e.InnerException.Message;
                }
            }
            List<Expense> expenses = expDal.Expenses.ToList<Expense>();
            Thread.Sleep(2000);
            return Json(expenses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriesByJson()
        {
            if (!Authorize())
                return RedirectToAction("RedirectByUser", "Home");
            return RedirectToAction("GetCategoriesByJson", "Manager");
        }
    }
}