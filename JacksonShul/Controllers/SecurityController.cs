using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public class AuthorizeUserAccessLevel : AuthorizeAttribute
    {

        public string UserRole { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string CurrentUserRole = HttpContext.Current.User.Identity.Name;
            if (CurrentUserRole == "yehoshualevin22@gmail.com")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}