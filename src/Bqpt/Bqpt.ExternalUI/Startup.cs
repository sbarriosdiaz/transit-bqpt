using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Bqpt.ExternalUI.Startup))]

namespace Bqpt.ExternalUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Authorize"),
                CookieSecure = CookieSecureOption.Always,
                CookieHttpOnly = true,
                CookieName = ".app.external",
                CookieSameSite = SameSiteMode.None,
                ExpireTimeSpan = TimeSpan.FromMinutes(20),
                LogoutPath = new PathString("/Account/Logoff")
            });
        }
    }
}