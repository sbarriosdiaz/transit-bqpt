////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BC.Identity.Kernel;
using Bqpt.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bqpt.WebUI.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;
        private ISqlConnectionProvider _connectionProvider;
        private AccountController controller;

        [TestInitialize]
        public void SetupTests()
        {
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity("userName"), new string[0]));
            moqRequest.Setup(x => x.UserHostAddress).Returns("::1");
            _connectionProvider = new SqlConnectionProvider();
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);

            controller = new AccountController(new IdentityApiClient(), new UserApplicationsService(_connectionProvider), new KernelSecurity(), signInManager.Object, authenticationManager.Object, new CurrentUserService());
        }

        [TestMethod()]
        public void AccountControllerTest()
        {
            controller.ControllerContext = new ControllerContext(moqContext.Object, new System.Web.Routing.RouteData(), controller);

            Assert.IsNotNull(controller);
        }

        [TestMethod()]
        public void LoginTest()
        {
            HttpContext.Current = CreateHttpContext(userLoggedIn: false);

            var result = controller.Login("~/") as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public async Task LoginTest1()
        {
            controller.ControllerContext = new ControllerContext(moqContext.Object, new System.Web.Routing.RouteData(), controller);

            var result = await controller.Login(new ApplicationLoginViewModel { UserIpAddress = "::1", Password = "password", Username = "username", ReturnUrl = string.Empty });
            var services = new UserApplicationsService(_connectionProvider);
            var apps = await services.MyApplicationsCard("cdiaz", "APP-00011");

            Assert.IsNotNull(result);
            Assert.IsTrue(!string.IsNullOrEmpty(apps));
        }

        [TestMethod()]
        public async Task ConnectorTest()
        {
            controller.ControllerContext = new ControllerContext(moqContext.Object, new System.Web.Routing.RouteData(), controller);

            var services = new UserApplicationsService(_connectionProvider);
            var _kernelSecurity = new KernelSecurity();
            var myapps = await services.MyApplicationsCard("cdiaz", "APP-00011");
            var resultWhenNotAuth = await controller.Connector(Guid.Empty.ToString());
            var resultWhenAuth = await controller.Connector("c5e1c36b-c543-4236-aea3-2c28460ce1aa");

            _kernelSecurity.SaveCookie("test-1", myapps, -1);
            _kernelSecurity.SaveCookie("test-2", "test", -1);
            _kernelSecurity.SaveCookie("test-3", "test", -1);

            Assert.IsTrue(!string.IsNullOrEmpty(myapps));
            Assert.IsTrue(_kernelSecurity != null);
            Assert.IsNotNull(resultWhenNotAuth);
            Assert.IsNotNull(resultWhenAuth);
        }

        [TestMethod()]
        public async Task LogOffTest()
        {
            controller.ControllerContext = new ControllerContext(moqContext.Object, new System.Web.Routing.RouteData(), controller);

            var services = new UserApplicationsService(_connectionProvider);
            var _kernelSecurity = new KernelSecurity();
            var myapps = await services.MyApplicationsCard("cdiaz", "APP-00011");

            _kernelSecurity.SaveCookie("test-1", myapps, -1);
            _kernelSecurity.SaveCookie("test-2", "test", -1);
            _kernelSecurity.SaveCookie("test-3", "test", -1);

            var result = await controller.LogOff("bye bye");

            Assert.IsTrue(!string.IsNullOrEmpty(myapps));
            Assert.IsTrue(_kernelSecurity != null);
            Assert.IsNotNull(result);
        }

        private static HttpContext CreateHttpContext(bool userLoggedIn)
        {
            var httpContext = new HttpContext(
                new HttpRequest(string.Empty, "http://sample.com", string.Empty),
                new HttpResponse(new StringWriter())
            )
            {
                User = userLoggedIn
                    ? new GenericPrincipal(new GenericIdentity("userName"), new string[0])
                    : new GenericPrincipal(new GenericIdentity(string.Empty), new string[0])
            };

            return httpContext;
        }
    }
}