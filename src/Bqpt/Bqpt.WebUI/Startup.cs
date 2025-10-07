////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Bqpt.WebUI.Startup))]

namespace Bqpt.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) => ConfigureAuth(app);
    }
}