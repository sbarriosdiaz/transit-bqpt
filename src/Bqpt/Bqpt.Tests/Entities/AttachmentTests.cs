////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System;
using Bqpt.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Bqpt.Domain.Tests
{
    [TestClass()]
    public class AttachmentTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var domainKey = Guid.NewGuid().ToString();
            var request = new Attachment
            {
                AttachmentId = domainKey,
                SetStatus = AttachmentStatus.Active,
                BlobUrl = "https://blobs",
                FileExtension = ".jpeg",
                FileName = "attachment.jpeg",
                FileSizeKb = 7890
            };

            var json = request.ToString();

            var other = JsonConvert.SerializeObject((Attachment)request.ToString());

            Assert.AreEqual(json, other);
            Assert.IsTrue(!string.IsNullOrEmpty(request.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(request.BlobUrl));
        }
    }
}