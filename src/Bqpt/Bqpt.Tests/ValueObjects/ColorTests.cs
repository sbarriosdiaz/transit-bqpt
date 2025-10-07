using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Domain.Tests
{
    [TestClass()]
    public class ColorTests
    {
        [TestMethod()]
        public void FromTest()
        {
            var code = "#FFFFFF";

            var color = Color.From(code);

            Assert.IsTrue(color.Code.Equals(code));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            string code = Color.White;

            Assert.IsTrue(code.Equals("#FFFFFF"));
        }
    }
}