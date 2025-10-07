////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using Bqpt.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Application.Tests
{
    [TestClass()]
    public class IdentityHelperTests
    {
        [TestMethod()]
        public void GetDomainNameTest()
        {
            var url = "www.broward.org";

            Assert.IsTrue(IdentityHelper.GetDomainName(url).Equals(".broward.org"));
        }
    }
}