////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using Bqpt.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ET.Identity.Tests
{
    [TestClass()]
    public class CurrentUserServiceTests
    {
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;

        [TestInitialize]
        public void SetupTests()
        {
            // Setup Moq
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            moqContext.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity("userName"), new string[] { "can.read" }));
            moqRequest.Setup(x => x.UserHostAddress).Returns("::1");
        }

        [TestMethod()]
        public void HasPermissionTest()
        {
            HttpContext.Current = CreateHttpContext(userLoggedIn: true);

            var service = new CurrentUserService();
            var userId = service.UserId;
            var isAuthenticated = service.IsAuthenticated;
            var username = service.UserName;
            var hasPermission = service.HasPermission("can.read");

            Assert.IsTrue(hasPermission);
            Assert.IsTrue(string.IsNullOrEmpty(userId));
            Assert.IsTrue(isAuthenticated);
            Assert.IsTrue(username.Equals("userName"));
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException), "Developer's Implementation of Roles Store")]
        public void RolesTest()
        {
            var service = new CurrentUserService();

            _ = service.UserPermissions();
        }

        private static HttpContext CreateHttpContext(bool userLoggedIn)
        {
            var httpContext = new HttpContext(
                new HttpRequest(string.Empty, "http://sample.com", string.Empty),
                new HttpResponse(new StringWriter())
            )
            {
                User = userLoggedIn
                    ? new GenericPrincipal(new GenericIdentity("userName"), roles: new string[] { "can.read" })
                    : new GenericPrincipal(new GenericIdentity(string.Empty), new string[0])
            };

            return httpContext;
        }
    }
}