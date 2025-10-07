////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace Bqpt.Infrastructure
{
    public class BaseEmailDto
    {
        private BaseEmailDto(MailAddress recipientList)
        {
        }

        public BaseEmailDto(IList<MailAddress> to, MailPriority mailPriority = MailPriority.Normal)
        {
            To = to;
            MailPriority = mailPriority;
        }

        public string From => $"{new MailAddress("noreply@broward.org", "Broward County NoReply")}";
        public MailPriority MailPriority { get; }
        public IList<MailAddress> To { get; }
        public IList<MailAddress> Cc { get; set; } = new List<MailAddress>();
        public IList<MailAddress> Bcc { get; set; } = new List<MailAddress>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public bool HasAttachment { get; set; }
        public string AttachmentFileName { get; set; }
        public Stream Attachment { get; set; }
    }
}