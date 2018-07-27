using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JacksonShul.Data;
using JacksonShul.Properties;

namespace JacksonShul
{
    public class LayoutDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repo = new VerifyRepository(Settings.Default.ConStr);
            Member member = repo.GetByEmail(HttpContext.Current.User.Identity.Name);
            filterContext.Controller.ViewBag.name = member != null ? member.FirstName : "";
            filterContext.Controller.ViewBag.fullName = member != null ? $"{member.FirstName} {member.LastName}"  : "";
            base.OnActionExecuting(filterContext);
        }
    }
}