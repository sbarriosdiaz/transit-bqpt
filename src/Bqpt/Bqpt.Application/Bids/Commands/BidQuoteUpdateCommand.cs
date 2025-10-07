using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class BidQuoteUpdateCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteUpdateCommand(BidQuoteViewModel form) => Form = form;

        public BidQuoteViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteUpdateCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;
            private readonly ISmtpServices _smtpServices;

            public Handler(IDbService dbService, ISmtpServices smtpServices)
            {
                _dbService = dbService;
                _smtpServices = smtpServices;
            }

            public async Task<TransactionResult<string>> Handle(BidQuoteUpdateCommand request, CancellationToken cancellationToken)
            {
                var normalizedQuoteId = request.Form.AssetWorksBidQuoteId.ToUpperInvariant();
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(b => b.AssetWorksBidQuoteId.Equals(normalizedQuoteId), cancellationToken);

                if (bidQuote is null) return new TransactionResult<string>(null, AppConstants.TransactionSuccess);

                bidQuote.BidScheduledEndTime = request.Form.BidScheduledEndTime;
                bidQuote.BidEndTime = null;
                bidQuote.SetStatus = BidQuoteStatus.Open;

                await _dbService.SaveChangesAsync(cancellationToken);
                // Send Notification Email to Vendors

                var emails = await _dbService.Set<Vendor>().AsNoTracking().ToListAsync(cancellationToken);
                foreach (var v in emails)
                {
                    var notificationEmail = AppConstants.BugNotificationEmail;

                    if (!AppConstants.IsDevelopment && !string.IsNullOrEmpty(v.AssetWorksVendorEmail))
                    {
                        notificationEmail = v.AssetWorksVendorEmail;
                    }
                    var mailNotificationAllUser = new BaseEmailDto(new List<MailAddress> { new MailAddress(notificationEmail) }, MailPriority.High)
                    {
                        Subject = "*** BQPT Mail Notification - A Bid Quote Has Been Updated***",
                        IsHtml = true,
                        Body = AppConstants.GetEmailTemplate($"<h2>Dear Customer: {v.AssetWorksVendorContactName}</h2> <br>The Bid Quote is now updated at: {bidQuote.BidScheduledStartTime}</br> <br> The Bid Quote will be closed: {bidQuote.BidScheduledEndTime}</br>")
                    };
                    _ = _smtpServices.SendEmail(mailNotificationAllUser, true);
                }

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}