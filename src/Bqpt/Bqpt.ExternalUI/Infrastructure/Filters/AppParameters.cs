////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using Bqpt.Common;

namespace BC
{
    public class AppParametersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];

            filterContext.Controller.ViewBag.ApplicationDesc = ConfigurationManager.AppSettings["ApplicationDesc"];

            filterContext.Controller.ViewBag.ApplicationVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            filterContext.Controller.ViewBag.AllowMobile = ConfigurationManager.AppSettings["AllowMobileView"];

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims
                                    .Where(c => c.Type.Equals(CustomClaimTypes.FirstName) || c.Type.Equals(CustomClaimTypes.LastName)).ToList();

                if (claims.Any())
                {
                    filterContext.Controller.ViewBag.WelcomeHeader = $"Welcome Back: {claims.First(c => c.Type.Equals(CustomClaimTypes.FirstName))?.Value} {claims.First(c => c.Type.Equals(CustomClaimTypes.LastName))?.Value}";
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}