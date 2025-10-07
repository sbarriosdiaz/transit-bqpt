using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.Application
{
    public class AttachmentQuery : IRequest<AttachmentViewModel>
    {
        public AttachmentQuery(string attachmentId)
        {
            AttachmentId = attachmentId;
        }

        public string AttachmentId { get; }

        public class Handler : IRequestHandler<AttachmentQuery, AttachmentViewModel>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService)
            {
                _dbService = dbService;
            }

            public async Task<AttachmentViewModel> Handle(AttachmentQuery request, CancellationToken cancellationToken)
            {
                var attachment = await _dbService.Set<Attachment>()
                   .AsNoTracking()
                   .FirstOrDefaultAsync(i => i.AttachmentId.Equals(request.AttachmentId));

                var attachmentviewmodel = new AttachmentViewModel
                {
                    AttachmentId = attachment.AttachmentId,
                    BlobUrl = attachment.BlobUrl,
                    FileName = attachment.FileName,
                    FileExtension = attachment.FileExtension,
                    FileSizeKb = attachment.FileSizeKb,
                    AttachmentFriendlyName = attachment.AttachmentFriendlyName,
                    Status = attachment.Status
                };

                return attachmentviewmodel;
            }
        }
    }
}