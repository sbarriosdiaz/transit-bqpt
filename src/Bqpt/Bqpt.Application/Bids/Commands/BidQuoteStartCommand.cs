using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class BidQuoteStartCommand : IRequest<TransactionResult<string>>
    {
        public BidQuoteStartCommand(BidQuoteViewModel form) => Form = form;

        public BidQuoteViewModel Form { get; }

        public class Handler : IRequestHandler<BidQuoteStartCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;
            private readonly ISmtpServices _smtpServices;

            public Handler(IDbService dbService, ISmtpServices smtpServices)
            {
                _dbService = dbService;
                _smtpServices = smtpServices;
            }

            private async Task CreateInPartVendors(int bidQuoteId, CancellationToken cancellationToken)
            {
                var bidQuoteParts = await _dbService.Set<BidQuotePart>().Where(p => p.BidQuoteId.Equals(bidQuoteId)).ToListAsync(cancellationToken);
                var systemVendors = await _dbService.Set<Vendor>().Where(x => x.IsActive).ToListAsync(cancellationToken);
                var vendorInPartsCheck = await _dbService.Set<VendorInPart>().Where(x => x.BidQuotePart.BidQuoteId.Equals(bidQuoteId)).ToListAsync(cancellationToken);

                var vendorInParts = new List<VendorInPart>();

                foreach (var p in bidQuoteParts)
                {
                    foreach (var v in systemVendors?.Where(x => x.CommodityCodes.Split(',').Contains(p.AssetWorksPartCommodityCode)).ToList())
                    {
                        if (vendorInPartsCheck.FirstOrDefault(x => x.BidQuotePartId.Equals(p.Id) && x.VendorId.Equals(v.Id)) == null)
                        {
                            vendorInParts.Add(new VendorInPart
                            {
                                BidQuotePart = p,
                                Vendor = v
                            });
                        }
                    }
                }

                _dbService.Set<VendorInPart>().AddRange(vendorInParts);
            }

            public async Task<TransactionResult<string>> Handle(BidQuoteStartCommand request, CancellationToken cancellationToken)
            {
                var normalizedQuoteId = request.Form.AssetWorksBidQuoteId.ToUpperInvariant();
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(b => b.AssetWorksBidQuoteId.Equals(normalizedQuoteId), cancellationToken);

                if (bidQuote is null) return new TransactionResult<string>(null, AppConstants.TransactionSuccess);

                await CreateInPartVendors(bidQuote.Id, cancellationToken);

                bidQuote.BidScheduledStartTime = DateTime.Now;
                bidQuote.BidStartTime = bidQuote.BidScheduledStartTime;
                bidQuote.BidScheduledEndTime = request.Form.BidScheduledEndTime;
                bidQuote.BidEndTime = null;
                bidQuote.SetStatus = BidQuoteStatus.Open;

                bidQuote.StatusHistories.Add(new BidQuoteStatusHistory
                {
                    Status = bidQuote.Status
                });

                //TODO: Remove prior to commit
                await _dbService.SaveChangesAsync(cancellationToken);

                var vendors = await _dbService.Set<Vendor>().AsNoTracking().ToListAsync(cancellationToken);
                foreach (var v in vendors)
                {
                    var notificationEmail = AppConstants.BugNotificationEmail;

                    if (!AppConstants.IsDevelopment && !string.IsNullOrEmpty(v.AssetWorksVendorEmail))
                    {
                        notificationEmail = v.AssetWorksVendorEmail;
                    }

                    var mailNotificationAllUser = new BaseEmailDto(new List<MailAddress> { new MailAddress(notificationEmail) }, MailPriority.High)
                    {
                        Subject = "*** BQPT Mail Notification - New Bid Quote Ready ***",
                        IsHtml = true,
                        Body = AppConstants.GetEmailTemplate($"<h2>Dear Customer: {v.AssetWorksVendorContactName}</h2> <br>A new Bid Quote is now open or has started at: {bidQuote.BidScheduledStartTime}</br> <br> The Bid Quote will be closed: {bidQuote.BidScheduledEndTime}</br>")
                    };

                   //TODO: remove prior to commit code
                   _ = _smtpServices.SendEmail(mailNotificationAllUser, true);
                }

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }
}