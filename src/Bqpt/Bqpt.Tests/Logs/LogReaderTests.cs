////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Linq;
using Bqpt.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Application.Tests
{
    [TestClass()]
    public class LogReaderTests
    {
        private ILogReader _reader;

        [TestInitialize]
        public void SetupTests() => _reader = new LogReader(new SqlConnectionProvider());

        [TestMethod()]
        public void AllLogsTest()
        {
            var logs = _reader.AllLogs().ToList();

            //Assert.IsTrue(logs.Any());
        }

        [TestMethod()]
        public void LatestLogTest()
        {
            var log = _reader.LatestLog();

            Assert.IsTrue(log != null);
        }

        [TestMethod()]
        public void LogsDashboardTest()
        {
            //var dash = _reader.LogsDashboard();

            //Assert.IsTrue(dash.Error != null);
            //Assert.IsTrue(dash.ErrorList.Any());
        }

        [TestMethod()]
        public void OneLogTest()
        {
            var log = _reader.OneLog(-1);

            Assert.IsTrue(log is null);
        }
    }
}