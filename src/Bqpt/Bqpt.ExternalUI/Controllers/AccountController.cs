using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BC;
using Bqpt.Application;
using Bqpt.Common;
using Bqpt.Infrastructure;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Bqpt.ExternalUI.Controllers
{
    public class AccountController : KernelControllerBase
    {
        public AccountController(IMediator mediator) => MediatR = mediator;

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Authorize()
        {
            var returnUrl = ConfigurationManager.AppSettings[AppConstants.AccessBrowardLocalConnectorKey].Contains("localhost")
                                                ? ConfigurationManager.AppSettings[AppConstants.AccessBrowardLocalConnectorKey].Replace("localhost", Request.Url.Host)
                                                : ConfigurationManager.AppSettings[AppConstants.AccessBrowardLocalConnectorKey];

            return new RedirectResult($"{ConfigurationManager.AppSettings[AppConstants.AccessBrowardKey]}/Login.aspx?ReturnUrl={returnUrl}");
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> Connector()
        {
            if (Request.Cookies[AppConstants.AccessBrowardCookie] != null)
            {
                var abTicket = FormsAuthentication.Decrypt(Request.Cookies[AppConstants.AccessBrowardCookie].Value);
                var userData = abTicket.UserData.Split('|').ToArray();

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userData[0].ToString()),
                        new Claim(ClaimTypes.NameIdentifier, userData[4].ToString() ),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(CustomClaimTypes.FirstName, $"{userData[1]}"),
                        new Claim(CustomClaimTypes.LastName, $"{userData[2]}")
                    };

                var validAccount = await MediatR.Send(new AccountQuery(userData[0].ToString()));

                if (validAccount != null)
                {
                    claims.Add(new Claim(CustomClaimTypes.CompanyName, $"{validAccount.AssetWorksVendorName}"));

                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                    var ctx = HttpContext.GetOwinContext();
                    var authenticationManager = ctx.Authentication;

                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);

                    IdentityHelper.DeleteABCookie();
                    IdentityHelper.ClearSessionId();

                    return RedirectToAction<HomeController>(a => a.Index());
                }
                else
                {
                    return RedirectToAction(nameof(UnAuthorized));
                }
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountRejected() => RedirectToAction(nameof(UnAuthorized));

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UnAuthorized() => View();

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logoff()
        {
            SignOutUser();

            return RedirectToAction<HomeController>(a => a.Index());
        }

        /// <summary>
        /// Will signout User as a private method to controller
        /// </summary>
        private void SignOutUser()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            IdentityHelper.DeleteABCookie();
            IdentityHelper.ClearSessionId();
        }
    }
}