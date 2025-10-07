////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Data.Entity.ModelConfiguration;
using Bqpt.Common;
using Bqpt.Domain;

namespace Bqpt.Persistence
{
    public class VendorPartBidHistoryConfiguration : EntityTypeConfiguration<VendorPartBidHistory>
    {
        public VendorPartBidHistoryConfiguration()
        {
            ToTable(nameof(VendorPartBidHistory));
            MapToStoredProcedures();

            Property(p => p.Comment).HasMaxLength(AppConstants.HasMaxLength1024);
        }
    }

    public class VendorPartBidSelectedHistoryConfiguration : EntityTypeConfiguration<VendorPartBidSelectedHistory>
    {
        public VendorPartBidSelectedHistoryConfiguration()
        {
            ToTable(nameof(VendorPartBidSelectedHistory));
            MapToStoredProcedures();
        }
    }

    public class VendorInPartConfiguration : EntityTypeConfiguration<VendorInPart>
    {
        public VendorInPartConfiguration()
        {
            ToTable(nameof(VendorInPart));
            MapToStoredProcedures();
        }
    }
}