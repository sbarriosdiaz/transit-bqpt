using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class ContactSubmitCommand : IRequest<TransactionResult<string>>
    {
        public string Email { get; }
        public string Message { get; }
        public string BidQuoteId { get; }
        public List<string> Files { get; }

        public ContactSubmitCommand(string bidQuoteId, string message, string email, List<string> files = null)
        {
            BidQuoteId = bidQuoteId;
            Message = message;
            Email = email;
            Files = files ?? new List<string>();
        }

        public class Handler : IRequestHandler<ContactSubmitCommand, TransactionResult<string>>
        {
            private readonly ISmtpServices _smtpServices;

            public Handler(ISmtpServices smtpServices)
            {
                _smtpServices = smtpServices;
            }

            public async Task<TransactionResult<string>> Handle(ContactSubmitCommand request, CancellationToken cancellationToken)
            {
                var subject = $"*** BQPT Mail Notification: BidQuote {request.BidQuoteId} ***";

                var recipients = new List<MailAddress>
                {
                    new MailAddress(request.Email),
                };               

                // Build the body with files (if any)
                var filesList = request.Files != null && request.Files.Count > 0
                    ? string.Join("<br>", request.Files)
                    : "No files were generated.";

                var body = $@"
                    <h2>Dear Customer,</h2>
                    <p>{request.Message}</p>
                    <p><strong>Bid Quote ID:</strong> {request.BidQuoteId}</p>
                    <p><strong>XML Files Generated:</strong><br>{filesList}</p>";

                var contactSubmission = new BaseEmailDto(recipients, MailPriority.Normal)
                {
                   
                    Subject = subject,
                    IsHtml = true,
                    Body = body
                };

                await Task.Run(() => _smtpServices.SendEmail(contactSubmission, true));

                return new TransactionResult<string>("OK", AppConstants.TransactionSuccess);
            }
        }
    }
}

