////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Web;
using Bqpt.Common;

namespace Bqpt.Infrastructure
{
    public static class IdentityHelper
    {
        public static void ClearSessionId()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            DeleteCookie(AppConstants.AspNetSessionIdCookie);
        }

        public static void DeleteCookie(string cookieName)
        {
            var host = HttpContext.Current.Request.Url.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped);

            var cookie = new HttpCookie(cookieName)
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(-1),
                Secure = true,
                Domain = GetDomainName(host)
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetDomainName(string url)
        {
            string domain;

            if (url.Contains("appdev"))
            {
                domain = ".appdev.cty";
            }
            else if (url.Contains("bctrain"))
            {
                domain = ".bctrain.cty";
            }
            else
            {
                domain = ".broward.org";
            }

            return domain;
        }

        public static void DeleteABCookie() => DeleteCookie(AppConstants.AccessBrowardCookie);
    }
}