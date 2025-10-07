////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.ExternalUI.Controllers.Tests
{
    [TestClass()]
    public class ErrorControllerTests
    {
        [TestMethod()]
        public void BrowserNotSupportedTest()
        {
            var controller = new ErrorsController();

            var result = controller.BrowserNotSupported() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void CustomErrorTest()
        {
            var controller = new ErrorsController();

            var result = controller.CustomError() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void NoJavascriptTest()
        {
            var controller = new ErrorsController();

            var result = controller.NoJavascript() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void NotFoundTest()
        {
            var controller = new ErrorsController();

            var result = controller.NotFound() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ServerErrorTest()
        {
            var controller = new ErrorsController();

            var result = controller.ServerError() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void TooManyRequestsTest()
        {
            var controller = new ErrorsController();

            var result = controller.TooManyRequests() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void UnAuthorizedTest()
        {
            var controller = new ErrorsController();

            var result = controller.UnAuthorized() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}

namespace Bqpt.WebUI.Controllers.Tests
{
    [TestClass()]
    public class ErrorControllerTests
    {
        [TestMethod()]
        public void BrowserNotSupportedTest()
        {
            var controller = new ErrorsController();

            var result = controller.BrowserNotSupported() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void CustomErrorTest()
        {
            var controller = new ErrorsController();

            var result = controller.CustomError() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void NoJavascriptTest()
        {
            var controller = new ErrorsController();

            var result = controller.NoJavascript() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void NotFoundTest()
        {
            var controller = new ErrorsController();

            var result = controller.NotFound() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ServerErrorTest()
        {
            var controller = new ErrorsController();

            var result = controller.ServerError() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void TooManyRequestsTest()
        {
            var controller = new ErrorsController();

            var result = controller.TooManyRequests() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void UnAuthorizedTest()
        {
            var controller = new ErrorsController();

            var result = controller.UnAuthorized() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}