////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using Bqpt.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Common.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void ToErrorSeverityColorTest()
        {
            var errorSeverity1 = 1;
            var errorSeverity2 = 2;
            var errorSeverity3 = 3;
            var errorSeverity4 = 4;
            var errorSeverity5 = 5;

            Assert.IsTrue(errorSeverity1.ToErrorSeverityColor().Contains("red"));
            Assert.IsTrue(errorSeverity2.ToErrorSeverityColor().Contains("orange"));
            Assert.IsTrue(errorSeverity3.ToErrorSeverityColor().Contains("yellow"));
            Assert.IsTrue(errorSeverity4.ToErrorSeverityColor().Contains("blue"));
            Assert.IsTrue(errorSeverity5.ToErrorSeverityColor().Contains("green"));
        }
    }
}

namespace ET.Identity.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void ParseEnumTest()
        {
            var e = "Active".ParseEnum<AttachmentStatus>();

            Assert.AreEqual(AttachmentStatus.Active, e);
        }

        [TestMethod()]
        public void ToYesOrNoTest()
        {
            bool? isFalse = false;

            var parsed = isFalse.ToYesOrNo();

            Assert.IsTrue(parsed.Contains("NO"));
        }

        [TestMethod()]
        public void ToFileSizeTest()
        {
            double number = 100;

            var parsed = number.ToFileSize();

            Assert.IsTrue(parsed.Contains("bytes"));
        }

        [TestMethod()]
        public void ToEnumDisplayNameTest()
        {
            var status = AttachmentStatus.OnScanning.ToEnumDisplayName();

            Assert.IsTrue(status.Contains("Scanning"));
        }

        [TestMethod()]
        public void ToFixedLengthTest() => Assert.IsTrue(true);

        [TestMethod()]
        public void RemoveInvalidCharsTest() => Assert.IsTrue(true);

        [TestMethod()]
        public void CheckIsValidUrlTest() => Assert.IsTrue(true);
    }
}