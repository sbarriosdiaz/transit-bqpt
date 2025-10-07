using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using FluentValidation;
using MediatR;

namespace Bqpt.Application
{
    public class DeleteAttachmentCommand : IRequest<IEnumerable<VendorBidQuoteAttachmentDto>>
    {
        public DeleteAttachmentCommand(VendorBidQuoteAttachmentDto payload) => Payload = payload;

        public VendorBidQuoteAttachmentDto Payload { get; }

        public class Handler : IRequestHandler<DeleteAttachmentCommand, IEnumerable<VendorBidQuoteAttachmentDto>>
        {
            private readonly IFilesConnectedServices _fileManager;
            private readonly IDbService _dbService;
            private readonly ICurrentUserService _currentUser;

            public Handler(IDbService dbService, IFilesConnectedServices fileManager, ICurrentUserService currentUser)
            {
                _dbService = dbService;
                _fileManager = fileManager;
                _currentUser = currentUser;
            }

            public async Task<IEnumerable<VendorBidQuoteAttachmentDto>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
            {
                var invoice = await _dbService.Set<VendorBidQuoteAttachment>()
                    .Include(a => a.Attachment)
                    .FirstOrDefaultAsync(o => o.AttachmentId.Equals(request.Payload.AttachmentId), cancellationToken);

                if (invoice is null) return null;

                var vendorId = invoice.VendorId;
                var attachment = invoice.Attachment;

                var deleteBlobResult = await _fileManager.DeleteFile(attachment.AttachmentId, cancellationToken);

                if (deleteBlobResult)
                {
                    attachment.SetStatus = AttachmentStatus.InActive;
                    attachment.IsActive = false;
                    attachment.IsDeleted = true;
                    attachment.DeletedBy = _currentUser.UserId;
                    attachment.DeletedOn = DateServices.DtNow;
                    invoice.IsActive = false;

                   // _dbService.Set<VendorBidQuoteAttachment>().Remove(invoice);

                    await _dbService.SaveChangesAsync(cancellationToken);
                }

                return await _dbService.Set<VendorBidQuoteAttachment>()
                                        .AsNoTracking()
                                        .Where(i => (i.Attachment.Status.Equals(nameof(AttachmentStatus.Active)) || i.Attachment.Status.Equals(nameof(AttachmentStatus.OnScanning))) && i.BidQuoteId == invoice.BidQuoteId &&
                                                   i.VendorId == vendorId)
                                       .Select(a => new VendorBidQuoteAttachmentDto
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
                                       }).ToListAsync();
            }
        }
    }
}