////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class VendorPartBidConfiguration : EntityTypeConfiguration<VendorPartBid>
    {
        public VendorPartBidConfiguration()
        {
            ToTable(nameof(VendorPartBid));
            MapToStoredProcedures();

            Property(p => p.Comment).HasMaxLength(AppConstants.HasMaxLength1024);

            HasMany(p => p.VendorPartBidSelectedHistories).WithRequired(p => p.VendorPartBid).HasForeignKey(p => p.VendorPartBidId).WillCascadeOnDelete(false);
        }
    }
}