////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using Bqpt.Common;
using Bqpt.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Bqpt.Tests
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void BaseEntityTest()
        {
            var domainKey = Guid.NewGuid().ToString();
            var request = new Attachment
            {
                AttachmentId = domainKey,
                SetStatus = AttachmentStatus.Active,
            };

            var json = JsonConvert.SerializeObject(request);

            var other = (Attachment)request.ToString();

            Assert.AreEqual(request.Status, AttachmentStatus.Active.ToString());
            Assert.AreNotEqual(request, other);

            Assert.AreEqual(json, request.ToString());
        }

        [TestMethod]
        public void BaseEntityTestForEquality()
        {
            var request1 = new Attachment
            {
                AttachmentId = Guid.NewGuid().ToString()
            };

            var request2 = new Attachment
            {
                AttachmentId = Guid.NewGuid().ToString()
            };

            Assert.IsTrue(request1 != request2);
            Assert.IsFalse(request1 == request2);
            Assert.IsFalse(request1.Equals(request2));
        }
    }
}