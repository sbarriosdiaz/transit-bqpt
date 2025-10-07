////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Persistence;
using Quartz;

namespace Bqpt.Infrastructure
{
    public class FileValidation : IJob
    {
        private readonly string _tenantId = AppConstants.TenantId;

        public async Task Execute(IJobExecutionContext context)
        {
            using (var dbContext = new AppDbContext())
            {
                var filesConnectedServices = new FilesConnectedServices();
                var cancellatonTokenSource = new CancellationTokenSource();
                var token = cancellatonTokenSource.Token;

                var nonValidatedFiles = await dbContext.Attachments
                                                        .Where(a => a.Status.Equals(AttachmentStatus.OnScanning.ToString()))
                                                        .ToListAsync(token);

                if (nonValidatedFiles.Any())
                {
                    var payload = nonValidatedFiles.Select(f => new FileValidationDto
                    {
                        DomainKey = f.AttachmentId,
                        TenantId = _tenantId
                    }).ToList();

                    var validatedFiles = await filesConnectedServices.FilesValidation(payload, token);

                    if (validatedFiles != null && validatedFiles.Any())
                    {
                        foreach (var file in validatedFiles)
                        {
                            if (file.IsActive)
                            {
                                var validatedFile = nonValidatedFiles.FirstOrDefault(f => f.AttachmentId.Equals(file.DomainKey));

                                validatedFile.SetStatus = AttachmentStatus.Active;
                            }
                        }

                        await dbContext.SaveChangesAsync(token);
                    }
                }
            }
        }
    }
}