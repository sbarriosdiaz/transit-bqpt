////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Bqpt.Common;
using Bqpt.Infrastructure;
using MediatR;
using Microsoft.Web.Mvc;

namespace BC
{
    [Authorize]
    [RequireHttps]
    public class KernelControllerBase : Controller
    {
        protected IMediator MediatR { get; set; }
        protected ICurrentUserService CurrentUser { get; set; }
        protected string SystemAccount => "10000000-1000-1000-10000000000000000";
        protected string TenantId => AppConstants.TenantId;
        protected string EIMSClientId => AppConstants.EIMSClientId;
        protected string EIMSClientSecret => AppConstants.EIMSClientSecret;
        protected string ApplicationName => AppConstants.ApplicationName;
        protected string ADApplicationName => AppConstants.AdApplicationName;
        protected string RequestIpAddress => string.IsNullOrEmpty(HttpContext.Request.UserHostAddress) ? AppConstants.LocalHostIpAddressDev : HttpContext.Request.UserHostAddress;

        protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
            where TController : Controller => ControllerExtensions.RedirectToAction(this, action);

        [Obsolete("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.")]
        protected JsonResult Json<T>(T data) => throw new InvalidOperationException("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.");

        protected KernelStandardJsonResult JsonValidationError()
        {
            var result = new KernelStandardJsonResult();

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }
            return result;
        }

        protected KernelStandardJsonResult JsonError(string errorMessage)
        {
            var result = new KernelStandardJsonResult();

            result.AddError(errorMessage);

            return result;
        }

        protected KernelStandardJsonResult<T> JsonSuccess<T>(T data) => new KernelStandardJsonResult<T> { Data = data };
    }
}