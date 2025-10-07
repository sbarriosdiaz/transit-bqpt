////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Bqpt.Common;
using Bqpt.ExternalUI.Controllers;
using Bqpt.Infrastructure;
using FluentValidation.Mvc;

namespace Bqpt.ExternalUI
{
    public class MvcApplication : HttpApplication
    {
        public MvcApplication()
        {
            PreSendRequestHeaders -= OnPreSendRequestHeaders;
            PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            

            FluentValidationModelValidatorProvider.Configure();
            AppScheduler.Start().GetAwaiter().GetResult();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AntiForgeryConfig.CookieName = $"_aft_{AppConstants.AdApplicationName}";
            AntiForgeryConfig.RequireSsl = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var applicationName = ConfigurationManager.AppSettings["ADApplicationName"].ToLowerInvariant();
            var logger = new BC.Monitors.Kernel.KernelLoggerFactories(applicationName);
            var exception = Server.GetLastError();
            var httpContext = ((MvcApplication)sender).Context;
            var routeData = new RouteData();

            if (exception is HttpException ex)
            {
                switch (ex.GetHttpCode())
                {
                    case 401:
                    case 403:
                        routeData.Values["controller"] = "Errors";
                        routeData.Values["action"] = "UnAuthorized";
                        logger.Save(Castle.Core.Logging.LoggerLevel.Info, applicationName, exception.Message, exception);
                        break;

                    case 404:

                        routeData.Values["controller"] = "Errors";
                        routeData.Values["action"] = "NotFound";
                       
                        break;

                    case 500:
                        routeData.Values["controller"] = "Errors";
                        routeData.Values["action"] = "ServerError";
                        logger.Save(Castle.Core.Logging.LoggerLevel.Fatal, applicationName, exception.Message, exception);
                        break;

                    default:
                        routeData.Values["controller"] = "Errors";
                        routeData.Values["action"] = "CustomError";
                        logger.Save(Castle.Core.Logging.LoggerLevel.Error, applicationName, exception.Message, exception);
                        break;
                }
            }
            else
            {
                routeData.Values["controller"] = "Errors";
                routeData.Values["action"] = "CustomError";

                logger.Save(Castle.Core.Logging.LoggerLevel.Fatal, applicationName, exception.Message, exception);
            }

            httpContext.ClearError();
            httpContext.Response.Clear();

            Response.TrySkipIisCustomErrors = true;

            IController errorController = new ErrorsController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}