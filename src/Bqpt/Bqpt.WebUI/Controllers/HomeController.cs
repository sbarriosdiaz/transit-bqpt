////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Web.Mvc;
using BC;
using Bqpt.Application;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.WebUI.Controllers
{
    public class HomeController : KernelControllerBase
    {
        private readonly IFilesConnectedServices _filesConnectedServices;

        public HomeController(IMediator mediator, IFilesConnectedServices filesConnectedServices)
        {
            MediatR = mediator;
            _filesConnectedServices = filesConnectedServices;
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => RedirectToAction(nameof(Dashboard));

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Dashboard() => View(await MediatR.Send(new BidQuoteDashboardQuery()));
    }
}