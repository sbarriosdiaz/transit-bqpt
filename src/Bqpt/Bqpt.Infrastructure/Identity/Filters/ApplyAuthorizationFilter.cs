////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Web.Mvc;

namespace Bqpt.Infrastructure
{
    public class ApplyAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                if (!filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Errors/UnAuthorized");
                }
            }
        }
    }
}