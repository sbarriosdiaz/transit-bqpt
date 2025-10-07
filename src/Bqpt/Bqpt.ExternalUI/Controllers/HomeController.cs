////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Web.Mvc;
using BC;

namespace Bqpt.ExternalUI.Controllers
{
    public class HomeController : KernelControllerBase
    {
        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index() => RedirectToAction("Index", "Bids");
    }
}