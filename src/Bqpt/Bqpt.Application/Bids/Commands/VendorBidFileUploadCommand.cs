using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class VendorBidFileUploadCommand : IRequest<TransactionResult<ICollection<VendorBidQuoteAttachmentDto>>>
    {
        public VendorBidFileUploadCommand(VendorBidQuoteAttachmentViewModel form) => Form = form;

        public VendorBidQuoteAttachmentViewModel Form { get; }

        public class Handler : IRequestHandler<VendorBidFileUploadCommand, TransactionResult<ICollection<VendorBidQuoteAttachmentDto>>>
        {
            private readonly IFilesConnectedServices _fileManager;
            private readonly IDbService _dbService;

            public Handler(IDbService dbService, IFilesConnectedServices fileManager)
            {
                _dbService = dbService;
                _fileManager = fileManager;
            }

            public async Task<TransactionResult<ICollection<VendorBidQuoteAttachmentDto>>> Handle(VendorBidFileUploadCommand request, CancellationToken cancellationToken)
            {
                await ArchiveAllInvoice(request.Form.VendorId, request.Form.BidQuoteId, cancellationToken);

                var invoicesFolder = $@"bidquotefiles\{request.Form.BidQuoteId}\vendor\{request.Form.VendorId}";

                var file = request.Form.File;
                var fileName = _fileManager.SetFileName(file.FileName);
                var fileSizeKb = file.ContentLength;

                if (file.ContentLength <= 0 || !CheckFileType(fileName)) return null;

                var blob = await _fileManager.SaveFile(file, invoicesFolder, fileName, cancellationToken);

                if (blob is null) return null;

                var bidQuote = await _dbService.Set<BidQuote>().FindAsync(request.Form.BidQuoteId);

                var normalizedFileFriendlyName = !string.IsNullOrEmpty(request.Form.FileFriendlyName)
                    ? request.Form.FileFriendlyName
                    : $"INV-{bidQuote.AssetWorksBidQuoteId}";

                var vendorBidAttachment = new VendorBidQuoteAttachment
                {
                    VendorId = request.Form.VendorId,
                    BidQuote = bidQuote,
                    Attachment = new Attachment
                    {
                        AttachmentId = blob.DomainKey,
                        BlobUrl = blob.CompleteBlobUrl.ToString(),
                        FileName = fileName,
                        FileExtension = Path.GetExtension(file.FileName).ToLowerInvariant(),
                        FileSizeKb = fileSizeKb,
                        AttachmentFriendlyName = normalizedFileFriendlyName,
                        SetStatus = AttachmentStatus.OnScanning
                    }
                };

                _dbService.Set<VendorBidQuoteAttachment>().Add(vendorBidAttachment);

                await _dbService.SaveChangesAsync(cancellationToken);

                var allInvoices = await _dbService.Set<VendorBidQuoteAttachment>()
                                                   .AsNoTracking()
                                                   .Include(a => a.Attachment)
                                                   .Where(i => i.Attachment.Status.Equals(nameof(AttachmentStatus.OnScanning)) &&
                                                                i.VendorId == request.Form.VendorId &&
                                                                i.BidQuoteId == bidQuote.Id)
                                                   .ToListAsync(cancellationToken);

                return new TransactionResult<ICollection<VendorBidQuoteAttachmentDto>>(allInvoices.Select(a => new VendorBidQuoteAttachmentDto
                {
                    AttachmentId = a.AttachmentId,
                    FileName = !string.IsNullOrEmpty(a.Attachment.AttachmentFriendlyName) ? $"{a.Attachment.AttachmentFriendlyName} (File Under Scanning Process)" : $"{a.Attachment.FileName} (File Under Scanning Process)",
                    FileExtension = a.Attachment.FileExtension,
                    FileFriendlyName = a.Attachment.AttachmentFriendlyName,
                    Status = a.Attachment.Status,
                    FileSize = a.Attachment.FileSizeKb,
                    CreatedOn = a.Attachment.CreatedOn
                }).ToList(), AppConstants.TransactionSuccess);
            }

            private async Task ArchiveAllInvoice(int vendorId, int bidQuoteid, CancellationToken cancellationToken)
            {
                var invoices = await _dbService.Set<VendorBidQuoteAttachment>()
                    .Include(a => a.Attachment)
                    .Where(bq => bq.Attachment.Status.Equals(nameof(AttachmentStatus.Active)) &&
                                 bq.BidQuoteId == bidQuoteid &&
                                 bq.VendorId == vendorId)
                    .ToListAsync(cancellationToken);

                foreach (var attachment in invoices.Select(a => a.Attachment))
                {
                    attachment.IsActive = false;
                    attachment.SetStatus = AttachmentStatus.Archived;
                }
            }

            private static bool CheckFileType(string fileName)
            {
                var extension = Path.GetExtension(fileName);

                switch (extension.ToLowerInvariant())
                {
                    case ".pdf":
                        return true;

                    default:
                        break;
                }

                return false;
            }
        }
    }
}