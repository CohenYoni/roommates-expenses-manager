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
            string groupStreet = objContext.Request.Form["Group.street"];
            string aptNumString = objContext.Request.Form["Group.aptNum"];
            //int? groupAptNum = aptNumString == "" ? null : (Int32)aptNumString;
            int? groupAptNum = null;
            if (aptNumString != null)
                groupAptNum = Int32.Parse(aptNumString);

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