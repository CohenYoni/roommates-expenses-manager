using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoommatesExpensesManager.ModelBinders
{
    public class GroupBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string groupCity = objContext.Request.Form["Group.city"];
            string groupStreet = objContext.Request.Form["Group.street"].ToString();
            int groupAptNum = Int32.Parse(objContext.Request.Form["Group.aptNum"]);

            Group obj = new Group()
            {
                city = groupCity,
                street = groupStreet,
                aptNum = groupAptNum
            };

            return obj;
        }
    }
}