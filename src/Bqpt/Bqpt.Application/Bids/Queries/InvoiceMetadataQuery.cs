using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class InvoiceMetadataQuery : IRequest<InvoiceDto>
    {
        public InvoiceMetadataQuery(string attachmentId) => AttachmentId = attachmentId;

        public string AttachmentId { get; }

        public class Handler : IRequestHandler<InvoiceMetadataQuery, InvoiceDto>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<InvoiceDto> Handle(InvoiceMetadataQuery request, CancellationToken cancellationToken) => await _dbService.Set<VendorBidQuoteAttachment>()
                     .Include(a => a.Attachment)
                     .Where(i => i.AttachmentId == request.AttachmentId)
                     .Select(d => new InvoiceDto
                     {
                         FileFriendlyName = d.Attachment.AttachmentFriendlyName,
                         BidQuoteId = d.BidQuoteId,
                         FileExtenstion = d.Attachment.FileExtension
                     }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}