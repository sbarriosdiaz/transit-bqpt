using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    internal class VendorBidQuoteAttachmentConfiguration : EntityTypeConfiguration<VendorBidQuoteAttachment>
    {
        public VendorBidQuoteAttachmentConfiguration()
        {
            ToTable(nameof(VendorBidQuoteAttachment));
            MapToStoredProcedures();

            Property(p => p.Comment).HasMaxLength(AppConstants.HasMaxLength1024);
        }
    }
}