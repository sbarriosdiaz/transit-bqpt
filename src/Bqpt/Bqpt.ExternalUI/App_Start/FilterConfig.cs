////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Web.Mvc;
using BC;

namespace Bqpt.ExternalUI
{
    public class FilterConfig
    {
        protected FilterConfig()
        {
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AppParametersAttribute());
            filters.Add(new RequireSecureConnectionFilterAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
}