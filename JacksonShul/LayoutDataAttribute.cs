using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JacksonShul.Data;

namespace JacksonShul
{
    public class LayoutDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repo = new VerifyRepository();
            Member member = repo.GetByEmail(HttpContext.Current.User.Identity.Name);
            filterContext.Controller.ViewBag.name = member != null ? member.FirstName : "";
            base.OnActionExecuting(filterContext);
        }
    }
}