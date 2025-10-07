////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bqpt.Infrastructure.Tests
{
    [TestClass()]
    public class SmtpServicesTests
    {
        private ISmtpServices _smtpService;

        [TestInitialize]
        public void SetupTests() => _smtpService = new SmtpServices();

        [TestMethod()]
        public void SendEmailTest()
        {
            var message = new BaseEmailDto(to: new List<MailAddress> { new MailAddress("systemsupportservices@broward.org") }, mailPriority: MailPriority.High)
            {
                Cc = new List<MailAddress> { new MailAddress("systemsupportservices@broward.org") },
                Bcc = new List<MailAddress> { new MailAddress("systemsupportservices@broward.org") },
                IsHtml = true,
                HasAttachment = true,
                Subject = "*** Unit Test Email Service",
                Body = "This is a test email"
            };

            var file = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}\\SMTP\\Services\\welcome-to-gitflow.pdf";

            using (var f = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                message.Attachment = f;
                message.AttachmentFileName = "welcome-to-gitflow.pdf";

                var result = _smtpService.SendEmail(message);

                Assert.IsTrue(result.Equals("sent"));
            }
        }

        [TestMethod()]
        public void PrepareHtmlTemplateTest()
        {
            var firstName = "John";
            var lastName = "Doe";
            var notificationTemplate = @"Welcome: <%FirstName%> <%LastName%>";

            var emailBody = _smtpService.PrepareHtmlTemplate(notificationTemplate, "FirstName", firstName, "LastName", lastName);

            Assert.IsTrue(emailBody.Equals("Welcome: John Doe"));
        }
    }
}