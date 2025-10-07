////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class AttachmentConfiguration : EntityTypeConfiguration<Attachment>
    {
        public AttachmentConfiguration()
        {
            ToTable(nameof(Attachment));
            MapToStoredProcedures();

            HasKey(p => p.AttachmentId);

            Property(e => e.AttachmentId).IsRequired().HasMaxLength(AppConstants.HasMaxLength48);
            Property(e => e.BlobUrl).IsRequired().HasMaxLength(AppConstants.HasMaxLength2048);
            Property(e => e.FileExtension).IsRequired().HasMaxLength(AppConstants.HasMaxLength10);
            Property(e => e.FileName).IsRequired().HasMaxLength(AppConstants.HasMaxLength128);
            Property(e => e.AttachmentFriendlyName).IsRequired().HasMaxLength(AppConstants.HasMaxLength256);
            Property(e => e.Status).IsRequired().HasMaxLength(AppConstants.HasDecimals18);
        }
    }
}