////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BC.Identity.Kernel;
using Bqpt.Common;
using Microsoft.AspNet.Identity;

namespace BC
{
    public class AppParametersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IKernelSecurity kernelSecurity = new KernelSecurity();

                if (string.IsNullOrEmpty(kernelSecurity.GetCookieString($"_aft_{AppConstants.AdApplicationName}", true)) || string.IsNullOrEmpty(kernelSecurity.GetCookieString($"common-name-{AppConstants.AdApplicationName}", true)) || string.IsNullOrEmpty(kernelSecurity.GetCookieString($"card-{AppConstants.AdApplicationName}", true)))
                {
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
                else
                {
                    filterContext.Controller.ViewBag.UserCommonName = kernelSecurity.GetCookieString($"common-name-{AppConstants.AdApplicationName}", true).ToClearString();
                    filterContext.Controller.ViewBag.Card = kernelSecurity.GetCookieString($"card-{AppConstants.AdApplicationName}", true).ToClearString();
                    filterContext.Controller.ViewBag.ApplicationName = AppConstants.ApplicationName;
                    filterContext.Controller.ViewBag.ApplicationDesc = AppConstants.ApplicationDescription;
                    filterContext.Controller.ViewBag.ApplicationVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    filterContext.Controller.ViewBag.AllowMobile = AppConstants.AllowMobileView;
                    filterContext.Controller.ViewBag.MyApps = kernelSecurity.GetCookieString($"apps-{AppConstants.AdApplicationName}", true);
                }
            }
            else
            {
                filterContext.Controller.ViewBag.ApplicationVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                filterContext.Controller.ViewBag.ApplicationName = AppConstants.ApplicationName;
                filterContext.Controller.ViewBag.ApplicationDesc = AppConstants.ApplicationDescription;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}