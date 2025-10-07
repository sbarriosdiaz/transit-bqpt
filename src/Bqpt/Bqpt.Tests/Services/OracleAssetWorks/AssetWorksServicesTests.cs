using Bqpt.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Application.Tests
{
    [TestClass()]
    public class AssetWorksServicesTests
    {
        private ISqlConnectionProvider _connProvider;
        private AssetWorksServices _service;

        [TestInitialize]
        public void SetupTests()
        {
            _connProvider = new SqlConnectionProvider();
            _service = new AssetWorksServices(_connProvider);
        }
    }
}