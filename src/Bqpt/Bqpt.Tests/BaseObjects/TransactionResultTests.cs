////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Common.Tests
{
    [TestClass()]
    public class TransactionResultTests
    {
        [TestMethod()]
        public void TransactionResultTest()
        {
            var result = new TransactionResult<bool>(true, "error");

            Assert.IsTrue(result.Result);
            Assert.IsTrue(result.Message.Equals("error"));
        }
    }
}