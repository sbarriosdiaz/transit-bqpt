using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.Identity;

namespace Bqpt.Application
{
    public class VendorBidQuoteAttachmentQuery : IRequest<VendorBidQuoteAttachmentResponseDto>
    {
        public VendorBidQuoteAttachmentQuery(string domainKey) => DomainKey = domainKey;

        public string DomainKey { get; }

        public class Handler : IRequestHandler<VendorBidQuoteAttachmentQuery, VendorBidQuoteAttachmentResponseDto>
        {
            private readonly IDbService _dbService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IDbService dbService, ICurrentUserService currentUserService)
            {
                _dbService = dbService;
                _currentUserService = currentUserService;
            }

            public async Task<VendorBidQuoteAttachmentResponseDto> Handle(VendorBidQuoteAttachmentQuery request, CancellationToken cancellationToken)
            {
                var vendorEmail = _currentUserService.UserName.ToLowerInvariant();

                var vendor = await _dbService.Set<Vendor>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.AssetWorksVendorEmail.Equals(vendorEmail), cancellationToken);

                if (vendor == null)
                {
                    return new VendorBidQuoteAttachmentResponseDto
                    {
                        Message = $"Vendor not found with email '{vendorEmail}'.",
                        VendorBidQuoteAttachmentForm = null,
                        VendorBidQuoteAttachments = Enumerable.Empty<VendorBidQuoteAttachmentDto>()
                    };
                }

                var invoices = await _dbService.Set<VendorBidQuoteAttachment>().AsNoTracking()
                    .Include(a => a.BidQuote)
                    .Include(a => a.Attachment)
                    .Where(i =>
                        (i.Attachment.Status.Equals(nameof(AttachmentStatus.Active)) ||
                         i.Attachment.Status.Equals(nameof(AttachmentStatus.OnScanning))) &&
                        i.BidQuote.DomainKey.Equals(request.DomainKey) &&
                        i.Vendor.Id == vendor.Id)
                    .ToListAsync(cancellationToken);

                BidQuote bidQuote;

                if (invoices.Any())
                {
                    var firstBidQuote = invoices.FirstOrDefault();
                    bidQuote = firstBidQuote?.BidQuote;
                }
                else
                {
                    bidQuote = await _dbService.Set<BidQuote>().AsNoTracking()
                        .FirstOrDefaultAsync(bq => bq.DomainKey.Equals(request.DomainKey), cancellationToken);
                }

                if (bidQuote == null)
                {
                    return new VendorBidQuoteAttachmentResponseDto
                    {
                        Message = $"BidQuote not found for domain key '{request.DomainKey}'.",
                        VendorBidQuoteAttachmentForm = null,
                        VendorBidQuoteAttachments = Enumerable.Empty<VendorBidQuoteAttachmentDto>()
                    };
                }

                var response = new VendorBidQuoteAttachmentResponseDto
                {
                    VendorBidQuoteAttachmentForm = new VendorBidQuoteAttachmentViewModel
                    {
                        BidQuoteId = bidQuote.Id,
                        VendorId = vendor.Id,
                    },
                    VendorBidQuoteAttachments = invoices.Any()
                        ? invoices.Select(a => new VendorBidQuoteAttachmentDto
                        {
                            AttachmentId = a.AttachmentId,
                            FileName = a.Attachment.FileName,
                            FileExtension = a.Attachment.FileExtension,
                            FileFriendlyName = a.Attachment.AttachmentFriendlyName,
                            Status = a.Attachment.Status,
                            VendorId = a.VendorId,
                            BidQuoteId = a.BidQuoteId,
                            FileSize = a.Attachment.FileSizeKb,
                            CreatedOn = a.Attachment.CreatedOn
                        }).ToList()
                        : Enumerable.Empty<VendorBidQuoteAttachmentDto>(),
                    Message = null
                };

                return response;
            }
        }
    }
}