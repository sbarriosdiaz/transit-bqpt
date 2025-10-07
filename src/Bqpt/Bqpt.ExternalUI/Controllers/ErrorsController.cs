////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Web.Mvc;

namespace Bqpt.ExternalUI.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        /// <summary>
        /// GET: Browser Not Supported Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult BrowserNotSupported() => View();

        /// <summary>
        /// GET: Custom Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomError() => View();

        /// <summary>
        /// GET: NO JS Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult NoJavascript() => View();

        /// <summary>
        /// GET: 404 Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound() => View();

        /// <summary>
        /// GET: 500 Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult ServerError() => View();

        /// <summary>
        /// GET: Too Many Request Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult TooManyRequests() => View();

        /// <summary>
        /// GET: 403 Error Redirect
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAuthorized() => View();
    }
}