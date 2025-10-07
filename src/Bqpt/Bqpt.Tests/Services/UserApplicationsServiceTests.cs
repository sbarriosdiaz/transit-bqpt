////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Linq;
using System.Threading.Tasks;
using Bqpt.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ET.Identity.Tests
{
    [TestClass()]
    public class UserApplicationsServiceTests
    {
        private ISqlConnectionProvider _connectionProvider;

        [TestInitialize]
        public void SetupTests() => _connectionProvider = new SqlConnectionProvider();

        [TestMethod()]
        public async Task MyApplicationsTest()
        {
            var services = new UserApplicationsService(_connectionProvider);
            var apps = await services.MyApplications("cdiaz", "APP-00011");

            Assert.IsTrue(apps.Any());
        }

        [TestMethod()]
        public async Task MyApplicationsCardTest()
        {
            var services = new UserApplicationsService(_connectionProvider);

            var apps = await services.MyApplicationsCard("cdiaz", "APP-00011");

            Assert.IsTrue(!string.IsNullOrEmpty(apps));
        }

        [TestMethod()]
        public async Task UserIdentityCardTest()
        {
            var services = new UserApplicationsService(_connectionProvider);

            var card = await services.UserIdentityCard("c5e1c36b-c543-4236-aea3-2c28460ce1aa");

            Assert.IsTrue(!string.IsNullOrEmpty(card));
        }

        [TestMethod()]
        public async Task ApplicationUserNameTest()
        {
            var services = new UserApplicationsService(_connectionProvider);

            var username = await services.ApplicationUserName("c5e1c36b-c543-4236-aea3-2c28460ce1aa");

            Assert.IsNotNull(username);
            Assert.IsTrue(username.ApplicationUserName.Equals("cdiaz"));
        }
    }
}