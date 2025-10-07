////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using BC;
using BC.Identity.Kernel;
using Bqpt.Common;
using Bqpt.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Bqpt.WebUI.Controllers
{
    public class AccountController : KernelControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IIdentityApiClient _identityApiClient;
        private readonly IKernelSecurity _kernelSecurity;
        private ApplicationSignInManager _signInManager;
        private readonly IUserApplicationsService _userApplicationsService;

        public AccountController(IIdentityApiClient identityApiClient,
                                 IUserApplicationsService applicationsService,
                                 IKernelSecurity kernelSecurity,
                                 ApplicationSignInManager signInManager,
                                 IAuthenticationManager authenticationManager,
                                 ICurrentUserService currentUser)
        {
            _identityApiClient = identityApiClient;
            _userApplicationsService = applicationsService;
            _signInManager = signInManager;
            AuthenticationManager = authenticationManager;
            _kernelSecurity = kernelSecurity;
            _currentUser = currentUser;
        }

        private IAuthenticationManager AuthenticationManager { get; }

        /// <summary>
        /// IDisposable
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _signInManager != null)
            {
                _signInManager.Dispose();
                _signInManager = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> Connector(string id)
        {
            var userInApplication = await _userApplicationsService.ApplicationUserName(id);

            if ((userInApplication != null && !userInApplication.TenantId.Equals(TenantId)) || userInApplication is null)
                return RedirectToAction(nameof(Login));

            var applicationUser = await _identityApiClient.GetApplicationUser(userInApplication.ApplicationUserName);

            if ((applicationUser != null && !applicationUser.EmailConfirmed) || applicationUser is null)
                return RedirectToAction(nameof(Login));

            await _signInManager.SignInAsync(applicationUser, false, false);
            await _identityApiClient.LogSignOn(applicationUser.Id, LoginActions.Login.ToString(), RequestIpAddress.Equals(AppConstants.LocalHostIpAddressDev)
                    ? AppConstants.LocalHostIpAddress
                    : RequestIpAddress);

            _kernelSecurity.SaveCookie($"common-name-{ADApplicationName}", $"{applicationUser.LastName}, {applicationUser.FirstName}".ToSecureHash(), -1);
            _kernelSecurity.SaveCookie($"card-{ADApplicationName}", await _userApplicationsService.UserIdentityCard(applicationUser.Id), -1);
            _kernelSecurity.SaveCookie($"apps-{ADApplicationName}", await _userApplicationsService.MyApplicationsCard(applicationUser.UserName, TenantId), -1);

            return RedirectToAction<HomeController>(a => a.Index());
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            CleanSupportCookies();

            return View(new ApplicationLoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AllowXRequestsEveryXSeconds(Name = "LogOn", ContentName = "TooManyRequests", Requests = 5, Seconds = 60)]
        public async Task<ActionResult> Login(ApplicationLoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            bool.TryParse(ConfigurationManager.AppSettings["DevMode"], out var isDebuggingEnabled);

            var authorization = await _identityApiClient.Authorize(new AuthorizationDto
            {
                UserName = vm.Username,
                Password = vm.Password,
                IpAddress = RequestIpAddress.Equals(AppConstants.LocalHostIpAddressDev) ? AppConstants.LocalHostIpAddress : RequestIpAddress,
                Environment = isDebuggingEnabled ? AuthEnvironment.Test : AuthEnvironment.Prod,
                AdApplicationName = ADApplicationName,
                TenantId = TenantId
            });

            if (authorization != null && authorization.IsAuthorized && authorization.HasErrors is false)
            {
                await _signInManager.SignInAsync(authorization.User, false, false);

                var myapps = await _userApplicationsService.MyApplicationsCard(vm.Username, TenantId);

                _kernelSecurity.SaveCookie($"common-name-{ADApplicationName}", $"{authorization.User.LastName}, {authorization.User.FirstName}".ToSecureHash(), -1);
                _kernelSecurity.SaveCookie($"card-{ADApplicationName}", authorization.IssuedCard.Card, -1);
                _kernelSecurity.SaveCookie($"apps-{ADApplicationName}", myapps, -1);

                return RedirectToLocal(vm.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("", authorization?.ErrorMessage ?? "Login unsuccessful");
            }

            return View(vm);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> LogOff(string message)
        {
            await _identityApiClient.LogSignOn(_currentUser.UserId, LoginActions.Logout.ToString(), RequestIpAddress.Equals(AppConstants.LocalHostIpAddressDev) ? AppConstants.LocalHostIpAddress : RequestIpAddress);

            CleanSupportCookies();

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction(nameof(Login));
        }

        #region Helpers

        /// <summary>
        /// Cleaning Support Cookies
        /// </summary>
        private void CleanSupportCookies()
        {
            _kernelSecurity.DeleteCookie($"common-name-{ADApplicationName}");
            _kernelSecurity.DeleteCookie($"card-{ADApplicationName}");
            _kernelSecurity.DeleteCookie($"apps-{ADApplicationName}");
        }

        /// <summary>
        /// GET: Helper to redirect only to local and prevent Open Redirection Attacks
        /// Refer: https://cheatsheetseries.owasp.org/cheatsheets/Unvalidated_Redirects_and_Forwards_Cheat_Sheet.html
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl) => Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction<HomeController>(a => a.Index());

        #endregion Helpers
    }
}