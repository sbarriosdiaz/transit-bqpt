////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Web.Mvc;

namespace BC
{
    public class RequireSecureConnectionFilterAttribute : RequireHttpsAttribute
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
        public class RequireHttpsAttribute : FilterAttribute, IAuthorizationFilter
        {
            public virtual void OnAuthorization(AuthorizationContext filterContext)
            {
                if (filterContext is null)
                {
                    throw new ArgumentNullException("Authorization Context is missing");
                }

                if (!filterContext.HttpContext.Request.IsLocal)
                {
                    if (filterContext.HttpContext.Request.IsSecureConnection)
                    {
                        return;
                    }

                    HandleNonHttpsRequest(filterContext);
                }
            }

            protected virtual void HandleNonHttpsRequest(AuthorizationContext filterContext)
            {
                if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("https is required");
                }

                var url = $"https://{filterContext.HttpContext.Request.Url.Host}{filterContext.HttpContext.Request.RawUrl}";

                filterContext.Result = new RedirectResult(url);
            }
        }
    }
}